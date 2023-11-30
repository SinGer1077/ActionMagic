using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

using Elements.Components;

namespace Elements.Systems
{
    partial struct ElementsSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ElementsSpawnerComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

        }
    }
}
