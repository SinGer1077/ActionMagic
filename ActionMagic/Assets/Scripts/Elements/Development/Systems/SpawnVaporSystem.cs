using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Physics.Systems;

using Elements.Data;
using Elements.Components;

namespace Elements.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct SpawnVaporSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BaseElementComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //var ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            //var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var job = new CreateVaporJob
            {
                ECB = ecb,
                Config = SystemAPI.GetSingleton<VaporConfigComponent>(),
                ConnectionsData = SystemAPI.GetBufferLookup<ElementConnection>(),
                TransformData = SystemAPI.GetComponentLookup<LocalTransform>(),
                ScaleData = SystemAPI.GetComponentLookup<PostTransformMatrix>()
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
       
        [BurstCompile]
        public partial struct CreateVaporJob : IJobEntity
        {
            public EntityCommandBuffer ECB;
            public VaporConfigComponent Config;
            public BufferLookup<ElementConnection> ConnectionsData;
            public ComponentLookup<LocalTransform> TransformData;
            public ComponentLookup<PostTransformMatrix> ScaleData;

            void Execute(Entity entity, ref BaseElementComponent element)
            {
                if (element.Type == ElementTypes.Water)
                {
                    var connections = ConnectionsData[entity];
                    for (int i = 0; i < connections.Length; i++)
                    {                        
                        if (connections[i].ConnectedElement.Type == ElementTypes.Fire && !connections[i].IsReacted)
                        {
                            var vaporEntity = ECB.Instantiate(Config.VaporPrefab);

                            float scaleValue = 1;
                            if (ScaleData.TryGetComponent(entity, out var scale))
                                scaleValue = scale.Value.c0.x * scale.Value.c2.z;

                            ECB.AddComponent(vaporEntity, new VaporComponent { Position = connections[i].ConnectionPosition, Radius = scaleValue, WaterElementEntity = entity} );

                            connections[i] = new ElementConnection(connections[i], true);
                        }
                    }
                }

                
            }
        }
    }
}
