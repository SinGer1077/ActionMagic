using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
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
            state.Dependency = new CollisionEventElementsJob
            {
                BaseElementData = SystemAPI.GetComponentLookup<BaseElementComponent>()
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
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

            float mass1 = manager.GetComponentData<WeightComponent>(first).WeightValue;
            float mass2 = manager.GetComponentData<WeightComponent>(second).WeightValue;

            float resultMass1 = Mathf.Clamp(mass1 - mass2 * ElementProrityTable.ElementPriorities[(int)secondElement.Type, (int)firstElement.Type], 0, Mathf.Infinity);
            float resultMass2 = Mathf.Clamp(mass2 - mass1 * ElementProrityTable.ElementPriorities[(int)firstElement.Type, (int)secondElement.Type], 0, Mathf.Infinity);

            manager.SetComponentData(first, new WeightComponent() { WeightValue = resultMass1 });
            manager.SetComponentData(second, new WeightComponent() { WeightValue = resultMass2 });
        }             

        [BurstCompile]
        public struct CollisionEventElementsJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentLookup<BaseElementComponent> BaseElementData;

            public void Execute(CollisionEvent collisionEvent)
            {
                Entity entityA = collisionEvent.EntityA;
                Entity entityB = collisionEvent.EntityB;

                bool isElementA = BaseElementData.HasComponent(entityA);
                bool isElementB = BaseElementData.HasComponent(entityB);

                if (isElementA && isElementB)
                {
                    ConnectElements(_manager, entityA, entityB, BaseElementData[entityA], BaseElementData[entityB]);
                }
            }
        }
    }


}
