using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics.Systems;
using UnityEngine;

using Elements.Components;
using Elements.Data;

namespace Elements.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct BaseElementsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BaseElementComponent>();           
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var setElementIdJob = new SetElementIdJob();
            var setElementIdJobHandle = setElementIdJob.Schedule(state.Dependency);
            setElementIdJobHandle.Complete();
        }

        [BurstCompile]
        public partial struct SetElementIdJob : IJobEntity
        {
            void Execute(Entity entity, ref BaseElementComponent element)
            {
                if (element.id.Index != entity.Index)
                {
                    element.id.Index = entity.Index;
                }
            }
        }
    }
}
