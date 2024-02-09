using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics.Systems;

using Elements.Components;
using Elements.Data;

using Universal.Components;

namespace Elements.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct PhysicsElementsSpawnerSystem : ISystem
    {
        private static EntityCommandBuffer _ecb;
        private static EntityManager _em;
        private static PhysicsElementsSpawnerComponent _spawner;

        private static int _currentType;
        private static int _currentWeight;
        private static int _currentXPos;
        private static float _cooldown;

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
            _ecb = new EntityCommandBuffer(Allocator.TempJob);
            _spawner = SystemAPI.GetSingleton<PhysicsElementsSpawnerComponent>();

            var flyingJob = new DynamicFlyingSpawnerJob
            {
                ECB = _ecb                
            };
            var flyHandle = flyingJob.Schedule(state.Dependency);
            flyHandle.Complete();                

            _em = state.EntityManager;          

            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }

        [BurstCompile]
        public partial struct DynamicFlyingSpawnerJob : IJobEntity
        {
            public EntityCommandBuffer ECB;
            //public EntityStorageInfoLookup EntityData;

            void Execute(Entity entity, ref PhysicsElementsSpawnerComponent spawner)
            {               
                if (_cooldown >= 0)
                {
                    _cooldown -= 0.03f;
                    return;
                }
                else
                {
                    _cooldown = 2.0f;
                }
                    

                if (_currentType == 0) _currentType = 1;
                else _currentType = 0;

                _currentWeight += 1;
                if (_currentWeight >= 20) _currentWeight = 0;

                Entity prefab = CreatePhysicsElement(_currentType, ECB);
                ECB.AddComponent(prefab, new LocalTransform
                {
                    Position = new float3(_currentXPos, spawner.SpawnPoint.y, spawner.SpawnPoint.z),
                    Rotation = Quaternion.identity,
                    Scale = _currentWeight * 0.1f
                });

                ECB.AddComponent(prefab, new WeightComponent { WeightValue = _currentWeight, InitWeightValue = _currentWeight,
                    Infinity = false, InitScale = _currentWeight * 0.1f });

                if (_currentXPos >= 20) _currentXPos = -20;
                else _currentXPos++;

                float timeToDestroy = 30.0f;
                ECB.AddComponent(prefab, new ShouldBeDestroyedComponent { MainEntity = prefab, Should = true, timerToDestroy = timeToDestroy });
                ECB.AddComponent(prefab, new TimerComponent { timer = timeToDestroy });
            }
        }

        public static Entity CreatePhysicsElement(int elementType, EntityCommandBuffer ecb)
        {
            var prefabs = _em.GetBuffer<ElementPrefab>(_spawner.Spawner);
            var elementsArray = _em.GetBuffer<ElementBuffer>(_spawner.Spawner);

            var entity = ecb.Instantiate(prefabs[(int)elementsArray[elementType].type].Prefab);          

            return entity;
        }

        public static Entity CreatePhysicsElement(Entity prefab, EntityCommandBuffer ecb)
        {            
            var entity = ecb.Instantiate(prefab);
            return entity;
        }
    }
}
