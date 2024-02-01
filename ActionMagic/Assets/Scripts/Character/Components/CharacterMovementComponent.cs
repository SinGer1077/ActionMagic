using Unity.Entities;

namespace Character.Components
{
    public struct CharacterMovementComponent : IComponentData
    {
        public Entity CharacterBody;
        public float MovementSpeed;
    }
}
