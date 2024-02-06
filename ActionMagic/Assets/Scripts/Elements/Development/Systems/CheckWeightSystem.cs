using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

using Elements.Components;
using Elements.Data;
using Universal.Components;

namespace Elements.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct CheckWeightSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<WeightComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //var ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            //var _ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
            var _ecb = new EntityCommandBuffer(Allocator.TempJob);
            var job = new AddDestroyComponentJob
            {
                ECB = _ecb
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();

            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }

        [WithNone(typeof(ShouldBeDestroyedComponent))]
        [BurstCompile]
        public partial struct AddDestroyComponentJob : IJobEntity
        {          
            public EntityCommandBuffer ECB;
            public BufferLookup<ElementConnection> connections;

            void Execute(Entity entity, ref WeightComponent weight)
            {
                if (weight.WeightValue <= 0)
                {
                    float timeToDestroy = 0.5f;
                    ECB.AddComponent(entity, new ShouldBeDestroyedComponent {MainEntity = entity, Should = true, timerToDestroy = timeToDestroy});
                    ECB.AddComponent(entity, new TimerComponent { timer = timeToDestroy });                    
                }
            }            
        }
    }
}
