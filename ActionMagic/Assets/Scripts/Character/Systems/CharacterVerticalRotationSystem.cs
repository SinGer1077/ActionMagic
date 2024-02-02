using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

using Character.Components;

namespace Character.Systems
{
    public partial struct CharacterVerticalRotationSystem : ISystem
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
                MouseDelta = Input.mousePosition                
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();
        }

        [BurstCompile]
        public partial struct RotationCharacterJob : IJobEntity
        {
            public Vector2 MouseDelta;            

            void Execute(Entity entity, ref CharacterVerticalRotationComponent rotation, ref LocalTransform transform)
            {
                rotation.Pitch = math.clamp(-MouseDelta.y * rotation.MouseSensivity, rotation.AngleBorders.x, rotation.AngleBorders.y);
                transform.Rotation = Quaternion.Euler(rotation.Pitch, 0.0f, 0.0f);                                              
            }
        }
    }
}
