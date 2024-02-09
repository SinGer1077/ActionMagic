using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Jobs;

using Universal.Components;

namespace Universal.Systems
{

    public partial struct RotationYSystem : ISystem
    {
        //public static JobHandle rotationHandler;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RotatedComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job1 = new RotateYJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,                
            };

            var job2 = new RotateXJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };

            
            var handle2  = job2.Schedule(state.Dependency);
            var handle1 = job1.Schedule(handle2);

            handle1.Complete();
            handle2.Complete();
        }

        [BurstCompile]
        partial struct RotateYJob : IJobEntity
        {
            public float deltaTime;
           
            void Execute(ref LocalTransform transform, in RotatedComponent rotation)
            {
                transform = transform.RotateY(rotation.RotationSpeed * deltaTime);
            }
        }

        [BurstCompile]
        partial struct RotateXJob : IJobEntity
        {
            public float deltaTime;

            void Execute(ref LocalTransform transform, in RotatedComponent rotation)
            {
                transform = transform.RotateY(rotation.RotationSpeed * -deltaTime * 0.5f);
            }
        }
    }
}
