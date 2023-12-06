using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

using Elements.Components;
using Elements.Data;

namespace Elements.Systems
{

    public partial struct BaseElementsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BaseElementComponent>();           
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            int count = 0;
            //var query = state.GetEntityQuery(typeof(BaseElementComponent));
            //count = query.CalculateEntityCount();
            ////foreach (var element in SystemAPI.Query<RefRO<BaseElementComponent>>())
            ////{
            ////    Debug.Log(element.ValueRO.Type);
            ////    count++;
            ////}
            var allEntities = state.EntityManager.GetAllEntities(Allocator.Persistent);
            int temp = 0;
            foreach (var ent in allEntities)
            {
                if (state.EntityManager.HasComponent(ent, typeof(BaseElementComponent)))
                {
                    temp++;
                }
            }
            Debug.Log(temp + " count count");
        }
    }
}
