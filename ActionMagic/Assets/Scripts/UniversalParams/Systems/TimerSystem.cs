using UnityEngine;

using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

using Universal.Components;

namespace Universal.Systems
{

    public partial struct TimerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TimerComponent>();            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var timer in SystemAPI.Query<RefRW<TimerComponent>>())
            {
                if (timer.ValueRO.timer > 0)
                {
                    timer.ValueRW.timer = math.clamp(timer.ValueRW.timer - Time.fixedDeltaTime, 0, timer.ValueRO.timer);
                }
            }
        }
    }
}
