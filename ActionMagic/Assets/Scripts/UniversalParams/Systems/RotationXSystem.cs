using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

using Universal.Components;

namespace Universal.Systems
{

    public partial struct RotationXSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RotatedComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //state.Dependency.Complete();

            //var job = new RotateXJob
            //{
            //    deltaTime = SystemAPI.Time.DeltaTime,
            //};
            //var handle = job.ScheduleParallel(state.Dependency);
            //handle.Complete();
        }

        [BurstCompile]
        partial struct RotateXJob : IJobEntity
        {
            public float deltaTime;

            void Execute(ref LocalTransform transform, in RotatedComponent rotation)
            {
                transform = transform.RotateY(rotation.RotationSpeed * deltaTime * -0.5f);
            }
        }
    }
}
