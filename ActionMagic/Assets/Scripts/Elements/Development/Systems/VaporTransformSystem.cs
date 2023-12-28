using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

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
        }
        
        [BurstCompile]
        public partial struct TransformVaporJob : IJobEntity
        {           
            void Execute(Entity entity, ref VaporComponent vapor, ref LocalTransform transform)
            {
                transform.Position = vapor.Position;
                //calculate particle radius
            }
        }
    }
}
