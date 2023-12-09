using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

using Elements.Components;
using Elements.Data;
using Universal.Components;

namespace Elements.Systems
{
    public partial struct ElementsSpawnerSystem : ISystem
    {
        private static EntityCommandBuffer _ecb;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ElementsSpawnerComponent>();
            _ecb = new EntityCommandBuffer(Allocator.Persistent);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            foreach (var spawner in SystemAPI.Query<ElementsSpawnerComponent>())
            {
                for (int i = 0; i < spawner.Count; i++)
                {
                    var entity = CreateElementEntity(state.EntityManager, (ElementTypes)Random.Range(0, 2), 10.0f);
                    //var entity = state.EntityManager.CreateEntity();
                    //_ecb.AddComponent(entity, new BaseElementComponent { Type = (ElementTypes)Random.Range(0, 2) });                  
                }
            }
            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }

        [BurstCompile]
        public static Entity CreateElementEntity(EntityManager manager, ElementTypes type, float weight)
        {           
            var entity = manager.CreateEntity(typeof(BaseElementComponent), typeof(ElementConnection), typeof(WeightComponent));
            manager.SetComponentData(entity, new BaseElementComponent { id = entity.Index, Type = type });
            manager.SetComponentData(entity, new WeightComponent { WeightValue = weight });
            return entity;
        }
    }
}
