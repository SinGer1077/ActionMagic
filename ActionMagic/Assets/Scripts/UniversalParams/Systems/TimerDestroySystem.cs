using UnityEngine;

using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

using Universal.Components;

namespace Universal.Systems
{
    public partial struct TimerDestroySystem : ISystem
    {
        private EntityCommandBuffer _ecb;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ShouldBeDestroyedComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {           
            _ecb = new EntityCommandBuffer(Allocator.TempJob);

            foreach (var (destroyComponent, timerComponent) in SystemAPI.Query<RefRW<ShouldBeDestroyedComponent>, RefRO<TimerComponent>>())
            {
                if (destroyComponent.ValueRO.Should == true)
                {
                    Debug.Log(destroyComponent.ValueRW.MainEntity.Index);
                    destroyComponent.ValueRW.timerToDestroy = timerComponent.ValueRO.timer;
                    if (destroyComponent.ValueRO.timerToDestroy <= 0)
                    {
                        _ecb.DestroyEntity(destroyComponent.ValueRW.MainEntity);
                    }
                }
            }

            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }
    }
}
