using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Physics;
using Unity.Entities.Graphics;
using Unity.Physics.Systems;

using Elements.Components;
using Elements.Data;

namespace Elements.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct VaporTransformSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<VaporComponent>();
        }

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
                    var main = particleSystem.Value.main;

                    shape.radius = vaporPrefab.Radius;
                    
                    if (vaporPrefab.Radius < 1.0)
                    {
                        main.startSize = vaporPrefab.Radius * 2.0f;
                        main.startSpeed = vaporPrefab.Radius * 2.0f;
                    }
                    else
                    {
                        main.startSize = vaporPrefab.Radius;
                        main.startSpeed = vaporPrefab.Radius;
                    }
                }
            }
        }
        
        public partial struct TransformVaporJob : IJobEntity
        {                  
            void Execute(Entity entity, ref VaporComponent vapor, ref LocalTransform transform)
            {
                transform.Position = vapor.Position;           
            }
        }       
    }
}
