using UnityEngine;

using Unity.Entities;

using Character.Components;

namespace Character.Authoring
{
    public class CharacterMovementAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private float _movementSpeed;

        class Baker : Baker<CharacterMovementAuthoring>
        {
            public override void Bake(CharacterMovementAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CharacterMovementComponent
                {
                    CharacterBody = GetEntity(authoring._character, TransformUsageFlags.Dynamic),
                    MovementSpeed = authoring._movementSpeed
                });
            }
        }
    }
}
