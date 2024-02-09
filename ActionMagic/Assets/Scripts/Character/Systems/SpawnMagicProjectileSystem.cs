using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

using Elements.Components;
using Elements.Data;
using Elements.Systems;

using Character.Components;

using Universal.Components;

namespace Character.Systems {

    public partial struct SpawnMagicProjectileSystem : ISystem
    {
        private EntityCommandBuffer _ecb;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CharacterControllerComponent>();
            _ecb = new EntityCommandBuffer(Allocator.Persistent);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _ecb = new EntityCommandBuffer(Allocator.Temp);
            var controller = SystemAPI.GetSingleton<CharacterControllerComponent>();
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {               
                controller.CurrentType = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                controller.CurrentType = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                controller.CurrentType = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                controller.CurrentType = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                controller.CurrentType = 4;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                controller.CurrentType = 5;
            }

            SystemAPI.SetSingleton(controller);
            var spellBook = SystemAPI.GetSingleton<MagicBookComponent>();
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                var spell = state.EntityManager.GetBuffer<SpellBuffer>(spellBook.SpellBook)[controller.CurrentType];

                //var projectile = PhysicsElementsSpawnerSystem.CreatePhysicsElement(controller.CurrentType, _ecb);
                var projectile = PhysicsElementsSpawnerSystem.CreatePhysicsElement(spell.SpellPrefab, _ecb);

                var characterTransform = state.EntityManager.GetComponentData<LocalToWorld>(controller.CameraTarget);
                var muzzleTransform = state.EntityManager.GetComponentData<LocalToWorld>(controller.SpawnAttackPosition);

                //float3 screenDirection = Camera.main.ScreenPointToRay(new Vector2(Screen.height / 2, Screen.width / 2)).direction;
                float3 cameraDirection = characterTransform.Forward;

                _ecb.AddComponent(projectile, new LocalTransform { Position = muzzleTransform.Position, Rotation = Quaternion.identity, Scale = 1.0f });
                _ecb.AddComponent(projectile, new WeightComponent { WeightValue = spell.ElementWeight, InitWeightValue = spell.ElementWeight,
                    Infinity = false, InitScale = 1.0f});                

                _ecb.AddComponent(projectile, new SimpleProjectileComponent
                    { Position = muzzleTransform.Position, Direction = cameraDirection, 
                    Created = false, FlySpeed = spell.FlySpeed, LifeTime = spell.LifeTime});
            }
            
            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();            
        }
    }
}
