using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

using Elements.Components;
using Elements.Data;

namespace Elements.Systems
{
    public partial struct PhysicsElementsSpawnerSystem : ISystem
    {
        private static EntityCommandBuffer _ecb;
        private static EntityManager _em;
        private static PhysicsElementsSpawnerComponent _spawner;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsElementsSpawnerComponent>();
            _ecb = new EntityCommandBuffer(Allocator.Persistent);
            _em = state.EntityManager;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //state.Enabled = false;
            _ecb = new EntityCommandBuffer(Allocator.Temp);
            _spawner = SystemAPI.GetSingleton<PhysicsElementsSpawnerComponent>();
            //var elementsArray = state.EntityManager.GetBuffer<ElementBuffer>(_spawner.Spawner);
           
            //var prefabs = state.EntityManager.GetBuffer<ElementPrefab>(_spawner.Spawner);

            //for (int i = 0; i < elementsArray.Length; i++)
            //{
            //    _ecb.Instantiate(prefabs[(int)elementsArray[i].type].Prefab);
            //}

            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();

            _em = state.EntityManager;
        }     
        
        public static Entity CreatePhysicsElement(int elementType, EntityCommandBuffer ecb)
        {
            var prefabs = _em.GetBuffer<ElementPrefab>(_spawner.Spawner);
            var elementsArray = _em.GetBuffer<ElementBuffer>(_spawner.Spawner);

            var entity = ecb.Instantiate(prefabs[(int)elementsArray[elementType].type].Prefab);          

            return entity;
        }
    }
}
