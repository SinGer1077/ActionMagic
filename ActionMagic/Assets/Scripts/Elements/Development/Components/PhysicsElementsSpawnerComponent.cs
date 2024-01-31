using Unity.Entities;
using Unity.Mathematics;

namespace Elements.Components
{
    public struct PhysicsElementsSpawnerComponent : IComponentData
    {
        public Entity Spawner;
        public float3 SpawnPoint;
    }
}
