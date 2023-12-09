using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

using Elements.Components;
using Elements.Data;

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
        }
    }
}
