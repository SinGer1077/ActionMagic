using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

using UnityEngine;

using Elements.Components;
using Elements.Data;
using Universal.Components;

namespace Elements.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct ElementsConnectionSystem : ISystem
    {
        private static EntityManager _manager;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BaseElementComponent>();
            _manager = state.EntityManager;
        }


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new CollisionEventElementsJob
            {
                BaseElementData = SystemAPI.GetComponentLookup<BaseElementComponent>(),
                WeightComponentData = SystemAPI.GetComponentLookup<WeightComponent>(),
                ConnectionsData = SystemAPI.GetBufferLookup<ElementConnection>()
            };
            var handle = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            handle.Complete();
        }

        [BurstCompile]
        public static void ConnectElements(EntityManager manager, Entity first, Entity second)
        {
            BaseElementComponent firstElement = manager.GetComponentData<BaseElementComponent>(first);
            BaseElementComponent secondElement = manager.GetComponentData<BaseElementComponent>(second);
            ConnectElements(manager, first, second, firstElement, secondElement);
        }

        [BurstCompile]
        public static void ConnectElements(EntityManager manager, Entity first, Entity second, BaseElementComponent firstElement, BaseElementComponent secondElement)
        {         
            DynamicBuffer<ElementConnection> connectionsFirst = manager.GetBuffer<ElementConnection>(first);
            DynamicBuffer<ElementConnection> connectionsSecond = manager.GetBuffer<ElementConnection>(second);

            foreach (ElementConnection connection in connectionsFirst)
            {
                if (connection.id == secondElement.id)
                {
                    return;
                }
            }

            connectionsFirst.Add(new ElementConnection(secondElement));
            connectionsSecond.Add(new ElementConnection(firstElement));
            Debug.Log("Дошёл сюда");
            float mass1 = manager.GetComponentData<WeightComponent>(first).WeightValue;
            float mass2 = manager.GetComponentData<WeightComponent>(second).WeightValue;

            float resultMass1 = Mathf.Clamp(mass1 - mass2 * ElementProrityTable.ElementPriorities[(int)secondElement.Type, (int)firstElement.Type], 0, Mathf.Infinity);
            float resultMass2 = Mathf.Clamp(mass2 - mass1 * ElementProrityTable.ElementPriorities[(int)firstElement.Type, (int)secondElement.Type], 0, Mathf.Infinity);

            manager.SetComponentData(first, new WeightComponent() { WeightValue = resultMass1 });
            manager.SetComponentData(second, new WeightComponent() { WeightValue = resultMass2 });
            Debug.Log("Дошёл сюда fsdad");
        }             

        [BurstCompile]
        public struct CollisionEventElementsJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentLookup<BaseElementComponent> BaseElementData;
            public ComponentLookup<WeightComponent> WeightComponentData;
            public BufferLookup<ElementConnection> ConnectionsData;
            public void Execute(CollisionEvent collisionEvent)
            {
                Entity entityA = collisionEvent.EntityA;
                Entity entityB = collisionEvent.EntityB;

                bool isElementA = BaseElementData.HasComponent(entityA);
                bool isElementB = BaseElementData.HasComponent(entityB);

                if (isElementA && isElementB)
                {
                    //ConnectElements( _manager, entityA, entityB, BaseElementData[entityA], BaseElementData[entityB]);
                    var firstElement = BaseElementData.GetRefRO(entityA);
                    var secondElement = BaseElementData.GetRefRO(entityB);

                    var connectionsFirst = ConnectionsData[entityA];
                    var connectionsSecond = ConnectionsData[entityB];

                    foreach (ElementConnection connection in connectionsFirst)
                    {
                        if (connection.id == secondElement.ValueRO.id)
                        {
                            return;
                        }
                    }

                    connectionsFirst.Add(new ElementConnection(secondElement.ValueRO));
                    connectionsSecond.Add(new ElementConnection(firstElement.ValueRO));

                    float mass1 = WeightComponentData.GetRefRW(entityA).ValueRW.WeightValue;
                    float mass2 = WeightComponentData.GetRefRW(entityB).ValueRW.WeightValue;

                    float resultMass1 = Mathf.Clamp(mass1 - mass2 * ElementProrityTable.ElementPriorities[(int)secondElement.ValueRO.Type, (int)firstElement.ValueRO.Type], 0, Mathf.Infinity);
                    float resultMass2 = Mathf.Clamp(mass2 - mass1 * ElementProrityTable.ElementPriorities[(int)firstElement.ValueRO.Type, (int)secondElement.ValueRO.Type], 0, Mathf.Infinity);

                    WeightComponentData.GetRefRW(entityA).ValueRW.WeightValue = resultMass1;
                    WeightComponentData.GetRefRW(entityB).ValueRW.WeightValue = resultMass2;                   
                }
            }
        }
    }


}
