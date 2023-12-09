using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

using Elements.Components;
using Elements.Data;
using Universal.Components;

namespace Elements.Systems
{

    public partial struct ElementsConnectionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BaseElementComponent>();
        }


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public static void ConnectElements(EntityManager manager, Entity first, Entity second)
        {
            BaseElementComponent firstElement = manager.GetComponentData<BaseElementComponent>(first);
            BaseElementComponent secondElement = manager.GetComponentData<BaseElementComponent>(second);
            DynamicBuffer<ElementConnection> connectionsFirst = manager.GetBuffer<ElementConnection>(first);
            DynamicBuffer<ElementConnection> connectionsSecond = manager.GetBuffer<ElementConnection>(second);

            connectionsFirst.Add(new ElementConnection(secondElement));
            connectionsSecond.Add(new ElementConnection(firstElement));

            float mass1 = manager.GetComponentData<WeightComponent>(first).WeightValue;
            float mass2 = manager.GetComponentData<WeightComponent>(second).WeightValue;            
            
            float resultMass1 = Mathf.Clamp(mass1 - mass2 * ElementProrityTable.ElementPriorities[(int)secondElement.Type, (int)firstElement.Type], 0, Mathf.Infinity);
            float resultMass2 = Mathf.Clamp(mass2 - mass1 * ElementProrityTable.ElementPriorities[(int)firstElement.Type, (int)secondElement.Type], 0, Mathf.Infinity); 

            manager.SetComponentData(first, new WeightComponent(){WeightValue = resultMass1 });
            manager.SetComponentData(second, new WeightComponent() { WeightValue = resultMass2 });
        }
    }
}
