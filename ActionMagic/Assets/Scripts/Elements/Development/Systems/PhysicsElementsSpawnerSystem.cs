using Unity.Entities;
using Unity.Burst;
using Unity.Collections;


using Elements.Components;
using Elements.Data;

namespace Elements.Systems
{
    public partial struct PhysicsElementsSpawnerSystem : ISystem
    {
        private static EntityCommandBuffer _ecb;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsElementsSpawnerSystem>();
            _ecb = new EntityCommandBuffer(Allocator.Persistent);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var spawner = SystemAPI.GetSingleton<PhysicsElementsSpawnerComponent>().Spawner;
            var elementsArray = state.EntityManager.GetBuffer<ElementBuffer>(spawner);
            var prefabs = state.EntityManager.GetBuffer<ElementPrefab>(spawner);

            for (int i = 0; i < elementsArray.Length; i++)
            {
                _ecb.Instantiate(prefabs[(int)elementsArray[i].type].Prefab);
            }

            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }      
    }
}
