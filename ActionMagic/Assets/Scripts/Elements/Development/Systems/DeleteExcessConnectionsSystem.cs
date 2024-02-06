using UnityEngine;

using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

using Elements.Components;
using Elements.Data;
using Universal.Components;

namespace Elements.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct DeleteExcessConnectionsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ElementConnection>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new DeleteConnectionsJob
            {                
                connectionsData = SystemAPI.GetBufferLookup<ElementConnection>(),
                EntityData = SystemAPI.GetEntityStorageInfoLookup()
            };
            var handle = job.Schedule(state.Dependency);
            handle.Complete();
        }

        [BurstCompile]
        [WithNone(typeof(ShouldBeDestroyedComponent))]
        public partial struct DeleteConnectionsJob : IJobEntity
        {            
            public BufferLookup<ElementConnection> connectionsData;
            public EntityStorageInfoLookup EntityData;

            void Execute(Entity entity, ref BaseElementComponent element)
            {
                for (int i = 0; i < connectionsData[entity].Length; i++)
                {
                    Entity connectedEntity = connectionsData[entity][i].ConnectedElement.id;
                    if (!EntityData.Exists(connectedEntity))
                    {
                        connectionsData[entity].RemoveAt(i);
                    }
                }
            }
        }
    }
}
