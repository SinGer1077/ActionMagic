using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Physics;
using Unity.Entities.Graphics;

using Elements.Components;
using Elements.Data;

namespace Elements.Systems
{
    public partial struct VaporTransformSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<VaporComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new TransformVaporJob
            {
                
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();

            foreach (var (particleSystem, parent) in SystemAPI.Query<SystemAPI.ManagedAPI.UnityEngineComponent<ParticleSystem>, Parent>())
            {
                if (state.EntityManager.HasComponent<VaporComponent>(parent.Value))
                {
                    var vaporPrefab = state.EntityManager.GetComponentData<VaporComponent>(parent.Value);
                    var shape = particleSystem.Value.shape;
                    shape.radius= vaporPrefab.Radius;
                }
            }
        }
        
        [BurstCompile]
        public partial struct TransformVaporJob : IJobEntity
        {                  
            void Execute(Entity entity, ref VaporComponent vapor, ref LocalTransform transform)
            {
                transform.Position = vapor.Position;           
            }
        }       
    }
}
