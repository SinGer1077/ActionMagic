using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

using Character.Components;

namespace Character.Systems
{

    public partial struct CharacterMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CharacterMovementComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            var job = new MoveCharacterJob
            {
                InputVector = new Vector2(horizontal, vertical)
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();
        }

        [BurstCompile]
        public partial struct MoveCharacterJob : IJobEntity
        {
            public Vector2 InputVector;

            void Execute(Entity entity, ref CharacterMovementComponent movement, ref LocalTransform transform, ref PhysicsVelocity velocity)
            {
                float3 verticalMovement = transform.Forward() * InputVector.y * movement.MovementSpeed;
                float3 horizontalMovement = transform.Right() * InputVector.x * movement.MovementSpeed;
                //velocity.Linear = new float3(movement.MovementSpeed * InputVector.x, 0f, movement.MovementSpeed * InputVector.y);
                velocity.Linear = verticalMovement + horizontalMovement;
            }
        }
    }
}
