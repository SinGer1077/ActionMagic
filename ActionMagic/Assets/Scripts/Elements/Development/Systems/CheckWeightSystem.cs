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
            var ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var job = new AddDestroyComponentJob
            {
                ECB = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged)
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();
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
