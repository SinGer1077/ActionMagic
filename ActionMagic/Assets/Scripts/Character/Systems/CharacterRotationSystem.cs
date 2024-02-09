using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;

using Character.Components;

namespace Character.Systems
{
    public partial struct CharacterRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CharacterRotationComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new RotationCharacterJob
            {
                //MouseDelta = Input.mousePosition
                MouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();
        }

        [BurstCompile]
        public partial struct RotationCharacterJob : IJobEntity
        {
            public Vector2 MouseDelta;

            void Execute(Entity entity, ref CharacterControllerComponent controller, ref CharacterRotationComponent rotation, ref LocalTransform transform)
            {
                rotation.Yaw += MouseDelta.x * rotation.MouseSensivity * 10.0f;
                transform.Rotation = Quaternion.Euler(0.0f, rotation.Yaw, 0.0f);             
            }
        }
    }
}
