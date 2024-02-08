using Unity.Entities;

namespace Character.Components
{

    public struct SpellBuffer : IBufferElementData
    {
        public Entity SpellPrefab;
        public float ElementWeight;
        public float FlySpeed;
        public float LifeTime;
    }
}
