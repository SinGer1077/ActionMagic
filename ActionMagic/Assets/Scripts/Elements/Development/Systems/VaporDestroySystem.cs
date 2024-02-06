using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Physics.Systems;

using Elements.Components;
using Elements.Data;

using Universal.Components;

namespace Elements.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
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
            //var ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var _ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

            var job = new DestroyVaporJob
            {
                //ECB = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged),
                ECB = _ecb,
                ShouldBeDestroyedData = SystemAPI.GetComponentLookup<ShouldBeDestroyedComponent>(),
                ConnectionsData = SystemAPI.GetBufferLookup<ElementConnection>(),
                EntityData = SystemAPI.GetEntityStorageInfoLookup()
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

            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();

        }

        [WithNone(typeof(ShouldBeDestroyedComponent))]
        [BurstCompile]
        public partial struct DestroyVaporJob : IJobEntity
        {
            public EntityCommandBuffer ECB;
            public ComponentLookup<ShouldBeDestroyedComponent> ShouldBeDestroyedData;
            public BufferLookup<ElementConnection> ConnectionsData;
            public EntityStorageInfoLookup EntityData;

            void Execute(Entity entity, ref VaporComponent vapor)
            {               
                if (!EntityData.Exists(vapor.WaterElementEntity))
                {
                    Debug.Log("Here");
                    AddDestroy(ECB, entity);
                    return;
                }                

                bool flag = false;
                foreach (var connect in ConnectionsData[vapor.WaterElementEntity])
                {
                    if (connect.ConnectedElement.Type == ElementTypes.Fire)
                        flag = true;
                }

                if (flag == false)
                {
                    AddDestroy(ECB, entity);
                    return;
                }


                if (ShouldBeDestroyedData.HasComponent(vapor.WaterElementEntity))
                {
                    if (ShouldBeDestroyedData[vapor.WaterElementEntity].Should == true)
                    {
                        AddDestroy(ECB, entity);
                    }
                }
            }

            private void AddDestroy(EntityCommandBuffer ECB, Entity entity)
            {
                float timeToDestroy = 10.0f;
                ECB.AddComponent(entity, new ShouldBeDestroyedComponent { MainEntity = entity, Should = true, timerToDestroy = timeToDestroy });
                ECB.AddComponent(entity, new TimerComponent { timer = timeToDestroy });
            }
        }
    }
}
