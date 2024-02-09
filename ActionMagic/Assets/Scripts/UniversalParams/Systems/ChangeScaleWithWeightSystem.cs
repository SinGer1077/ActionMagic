using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

using Universal.Components;

namespace Universal.System
{
    public partial struct ChangeScaleWithWeightSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<WeightComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new ChangeScaleWithWeightJob();
            var jobHandle = job.Schedule(state.Dependency);
            jobHandle.Complete();
        }

        [BurstCompile]
        public partial struct ChangeScaleWithWeightJob : IJobEntity
        {
            void Execute(Entity entity, ref WeightComponent weight, ref LocalTransform transform)
            {
                if (weight.WeightValue != weight.InitWeightValue)
                {                    
                    float percentage = weight.WeightValue / weight.InitWeightValue;
                    transform.Scale = Mathf.Lerp(weight.InitScale, weight.InitScale * percentage, weight.LerpTimer / 0.5f);                    
                    weight.LerpTimer += 0.03f;
                }
            }
        }
    }
}
