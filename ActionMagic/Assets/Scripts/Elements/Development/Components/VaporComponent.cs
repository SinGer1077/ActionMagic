using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;

namespace Elements.Components
{
    public struct VaporComponent : IComponentData
    {
        public float Radius;
        public float3 Position;
        public Entity WaterElementEntity;
    }
}
