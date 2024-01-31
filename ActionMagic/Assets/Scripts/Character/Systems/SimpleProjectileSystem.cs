using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Physics;

using Character.Components;

namespace Character.Systems
{
    public partial struct SimpleProjectileSystem : ISystem
    {        

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimpleProjectileComponent>();           
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {            
            var spawnJob = new SpawnProjectileJob();           
            var spawnJobHandle = spawnJob.Schedule(state.Dependency);
            spawnJobHandle.Complete();

            var flyJob = new FlyProjectileJob();
            var flyJobHandle = flyJob.Schedule(spawnJobHandle);
            flyJobHandle.Complete();
        }

        [BurstCompile]
        public partial struct SpawnProjectileJob : IJobEntity
        {
            void Execute(Entity entity, ref SimpleProjectileComponent projectile, ref LocalTransform transform)
            {
                if (projectile.Created == false)
                {
                    transform.Position = projectile.Position;
                    projectile.Created = true;
                }
            }
        }

        [BurstCompile]
        public partial struct FlyProjectileJob : IJobEntity
        {
            void Execute(Entity entity, ref SimpleProjectileComponent projectile, ref PhysicsVelocity velocity)
            {
                if (projectile.Created == true)
                {
                    velocity.Linear = projectile.Direction * projectile.FlySpeed;
                }
            }
        }
    }
}
