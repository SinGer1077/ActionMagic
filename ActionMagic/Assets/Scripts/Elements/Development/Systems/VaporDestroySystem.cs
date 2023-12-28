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
                        //плавное удаление пара, а пока удалю просто
                        float timeToDestroy = 1.5f;
                        ECB.AddComponent(entity, new ShouldBeDestroyedComponent { MainEntity = entity, Should = true, timerToDestroy = timeToDestroy });
                        ECB.AddComponent(entity, new TimerComponent { timer = timeToDestroy });
                    }
                }
            }
        }
    }
}
