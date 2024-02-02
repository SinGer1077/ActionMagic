using UnityEngine;

using Unity.Entities;

using Character.Components;

namespace Character.Authoring
{
    public class CharacterVerticalRotationAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float _mouseSensivity;

        [SerializeField]
        private Vector2 _angleBorders;

        class Baker : Baker<CharacterVerticalRotationAuthoring>
        {
            public override void Bake(CharacterVerticalRotationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CharacterVerticalRotationComponent
                {
                    Pitch = 0.0f,                    
                    MouseSensivity = authoring._mouseSensivity,
                    AngleBorders = authoring._angleBorders
                });
            }
        }
    }
}
