using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics.Systems;

using Elements.Data;
using Elements.Components;

using Universal.Components;

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
            state.RequireForUpdate<WeightComponent>();
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
                ScaleData = SystemAPI.GetComponentLookup<PostTransformMatrix>(),
                WeightData = SystemAPI.GetComponentLookup<WeightComponent>()
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
            public ComponentLookup<WeightComponent> WeightData;

            void Execute(Entity entity, ref BaseElementComponent element)
            {
                if (element.Type == ElementTypes.Water)
                {
                    var connections = ConnectionsData[entity];
                    for (int i = 0; i < connections.Length; i++)
                    {
                        if (connections[i].ConnectedElement.Type == ElementTypes.Fire && !connections[i].IsReacted)
                        {
                            Entity smallerEntity = entity;

                            if (WeightData[connections[i].ConnectedElement.id].WeightValue <= WeightData[entity].WeightValue)
                            {
                                smallerEntity = connections[i].ConnectedElement.id;
                            }

                            var vaporEntity = ECB.Instantiate(Config.VaporPrefab);

                            float scaleValue = 1;
                            if (ScaleData.TryGetComponent(smallerEntity, out var scale))
                                scaleValue = math.max(scale.Value.c0.x, scale.Value.c2.z) / 2.0f;

                            ECB.AddComponent(vaporEntity, new VaporComponent { Position = connections[i].ConnectionPosition, Radius = scaleValue, WaterElementEntity = entity });

                            connections[i] = new ElementConnection(connections[i], true);
                        }
                    }
                }

                
            }
        }
    }
}
