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

            SystemAPI.SetSingleton(controller);
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                var projectile = PhysicsElementsSpawnerSystem.CreatePhysicsElement(controller.CurrentType, _ecb);
                var characterTransform = state.EntityManager.GetComponentData<LocalTransform>(controller.CharacterParent);
                var muzzleTransform = state.EntityManager.GetComponentData<LocalToWorld>(controller.SpawnAttackPosition);
                _ecb.AddComponent(projectile, new SimpleProjectileComponent
                    { Position = muzzleTransform.Position, Direction = characterTransform.Forward(), Created = false, FlySpeed = 10.0f }); 
                    //добавить лайфтайм для снарядов, сделать это в виде конфигов
            }
            
            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }
    }
}
