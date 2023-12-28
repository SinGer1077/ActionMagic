using System;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
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
                ConnectionsData = SystemAPI.GetBufferLookup<ElementConnection>(),
                physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld
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

            var mass1 = manager.GetComponentData<WeightComponent>(first);
            var mass2 = manager.GetComponentData<WeightComponent>(second);

            float resultMass1 = ChangeWeight(mass1, mass2, firstElement.Type, secondElement.Type);
            float resultMass2 = ChangeWeight(mass2, mass1, secondElement.Type, firstElement.Type);

            manager.SetComponentData(first, new WeightComponent() { WeightValue = resultMass1 });
            manager.SetComponentData(second, new WeightComponent() { WeightValue = resultMass2 });
        }             

        [BurstCompile]
        private static float ChangeWeight(WeightComponent firstWeight, WeightComponent secondWeight, ElementTypes firstType, ElementTypes secondType)
        {
            float resultMass1 = firstWeight.WeightValue;
            if (firstWeight.Infinity == false)
            {
                resultMass1 = Mathf.Clamp(firstWeight.WeightValue - secondWeight.WeightValue
                    * ElementProrityTable.ElementPriorities[(int)secondType, (int)firstType], 0, Mathf.Infinity) * 1 - Convert.ToInt32(secondWeight.Infinity);
            }
            return resultMass1;
        }        

        [BurstCompile]
        public struct CollisionEventElementsJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentLookup<BaseElementComponent> BaseElementData;
            public ComponentLookup<WeightComponent> WeightComponentData;
            public BufferLookup<ElementConnection> ConnectionsData;
            public PhysicsWorld physicsWorld; 
            public void Execute(CollisionEvent collisionEvent)
            {
                Entity entityA = collisionEvent.EntityA;
                Entity entityB = collisionEvent.EntityB;

                float3 contactPoint = collisionEvent.CalculateDetails(ref physicsWorld).AverageContactPointPosition;
                ConnectElements(entityA, entityB, contactPoint);
            }

            public void ConnectElements(Entity entityA, Entity entityB, float3 contactPoint)
            {
                bool isElementA = BaseElementData.HasComponent(entityA);
                bool isElementB = BaseElementData.HasComponent(entityB);

                if (isElementA && isElementB)
                {
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
                    
                    connectionsFirst.Add(new ElementConnection(secondElement.ValueRO, contactPoint));
                    connectionsSecond.Add(new ElementConnection(firstElement.ValueRO, contactPoint));

                    var mass1 = WeightComponentData.GetRefRW(entityA).ValueRW;
                    var mass2 = WeightComponentData.GetRefRW(entityB).ValueRW;

                    float resultMass1 = ChangeWeight(mass1, mass2, firstElement.ValueRO.Type, secondElement.ValueRO.Type);
                    float resultMass2 = ChangeWeight(mass2, mass1, secondElement.ValueRO.Type, firstElement.ValueRO.Type);

                    WeightComponentData.GetRefRW(entityA).ValueRW.WeightValue = resultMass1;
                    WeightComponentData.GetRefRW(entityB).ValueRW.WeightValue = resultMass2;
                }
            }
        }
    }


}
