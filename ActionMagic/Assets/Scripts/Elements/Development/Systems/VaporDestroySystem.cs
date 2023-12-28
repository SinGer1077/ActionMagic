using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

using Elements.Components;
using Elements.Data;

using Universal.Components;

namespace Elements.Systems
{
    public partial struct VaporDestroySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<VaporComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var job = new DestroyVaporJob
            {
                ECB = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged),
                ShouldBeDestroyedData = SystemAPI.GetComponentLookup<ShouldBeDestroyedComponent>()
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();

            foreach (var (particleSystem, parent) 
                in SystemAPI.Query<SystemAPI.ManagedAPI.UnityEngineComponent<ParticleSystem>, Parent>())
            {
                if (state.EntityManager.HasComponent<VaporComponent>(parent.Value))
                {
                    if (state.EntityManager.HasComponent<ShouldBeDestroyedComponent>(parent.Value))
                    {
                        var shouldDestroy = state.EntityManager.GetComponentData<ShouldBeDestroyedComponent>(parent.Value);
                        if (shouldDestroy.Should == true)
                        {
                            var main = particleSystem.Value.main;
                            main.loop = false;                            
                        }
                    }
                }
            }

        }

        [WithNone(typeof(ShouldBeDestroyedComponent))]
        [BurstCompile]
        public partial struct DestroyVaporJob : IJobEntity
        {
            public EntityCommandBuffer ECB;
            public ComponentLookup<ShouldBeDestroyedComponent> ShouldBeDestroyedData;

            void Execute(Entity entity, ref VaporComponent vapor)
            {
                if (ShouldBeDestroyedData.HasComponent(vapor.WaterElementEntity))
                {
                    if (ShouldBeDestroyedData[vapor.WaterElementEntity].Should == true)
                    {
                        float timeToDestroy = 10.0f;
                        ECB.AddComponent(entity, new ShouldBeDestroyedComponent { MainEntity = entity, Should = true, timerToDestroy = timeToDestroy });
                        ECB.AddComponent(entity, new TimerComponent { timer = timeToDestroy });
                    }
                }
            }
        }
    }
}
