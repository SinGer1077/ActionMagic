using Unity.Entities;

namespace Universal.Components
{
    public struct ShouldBeDestroyedComponent : IComponentData
    {
        public bool Should;
        public float timerToDestroy;
        public Entity MainEntity;
    }
}
