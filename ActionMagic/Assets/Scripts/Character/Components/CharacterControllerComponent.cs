using Unity.Entities;
using Unity.Mathematics;

namespace Character.Components
{
    public struct CharacterControllerComponent : IComponentData
    {
        public int CurrentType;
        public Entity CharacterParent;
        public Entity SpawnAttackPosition;
        public Entity CameraTarget;
    }
}
