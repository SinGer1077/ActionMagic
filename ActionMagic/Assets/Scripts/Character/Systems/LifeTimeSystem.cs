using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;

using Character.Components;

namespace Character.Systems
{
    public partial struct LifeTimeSystem : ISystem
    {
        private EntityCommandBuffer _ecb;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimpleProjectileComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _ecb = new EntityCommandBuffer(Allocator.TempJob);

            var job = new LifeTimeJob()
            {
                ECB = _ecb
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();

            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }
        
        [BurstCompile]
        public partial struct LifeTimeJob : IJobEntity
        {
            public EntityCommandBuffer ECB;            

            void Execute(Entity entity, ref SimpleProjectileComponent projectile)
            {
                projectile.LifeTime -= 0.03f;

                if (projectile.LifeTime <= 0)
                {
                    ECB.DestroyEntity(entity);
                }
            }
        }
    }
}
