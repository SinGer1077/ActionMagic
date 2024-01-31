using Unity.Entities;
using Unity.Mathematics;

namespace Character.Components
{
    public struct SimpleProjectileComponent : IComponentData
    {
        public float3 Position;
        public float3 Direction;
        public bool Created;
        public float FlySpeed;
    }
}
