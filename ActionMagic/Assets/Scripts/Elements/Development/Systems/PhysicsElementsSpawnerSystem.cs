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
        private EntityCommandBuffer _ecb;
        private EntityManager _em;
        private PhysicsElementsSpawnerComponent _spawner;

        private static int _currentType;
        private static int _currentWeight;
        private static int _currentXPos;
        private static float _cooldown;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsElementsSpawnerComponent>();
        
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //_em = state.EntityManager;
            _ecb = new EntityCommandBuffer(Allocator.TempJob);
            //_spawner = SystemAPI.GetSingleton<PhysicsElementsSpawnerComponent>();

            var flyingJob = new DynamicFlyingSpawnerJob
            {
                ECB = _ecb,
                manager = state.EntityManager,
                Spawner = SystemAPI.GetSingleton<PhysicsElementsSpawnerComponent>()
            };
            var flyHandle = flyingJob.Schedule(state.Dependency);
            flyHandle.Complete();                

            _em = state.EntityManager;          

            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }

        public partial struct DynamicFlyingSpawnerJob : IJobEntity
        {
            public EntityCommandBuffer ECB;
            public EntityManager manager;
            public PhysicsElementsSpawnerComponent Spawner;

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

                Entity prefab = CreatePhysicsElement(_currentType, ECB, manager, spawner);
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

        public static Entity CreatePhysicsElement(int elementType, EntityCommandBuffer ecb, EntityManager em, PhysicsElementsSpawnerComponent spawner)
        {
            var prefabs = em.GetBuffer<ElementPrefab>(spawner.Spawner);
            var elementsArray = em.GetBuffer<ElementBuffer>(spawner.Spawner);

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
