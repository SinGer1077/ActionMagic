using UnityEngine;

using Unity.Entities;

using Character.Components;

namespace Character.Authoring
{
    public class CharacterRotationAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float _mouseSensivity;

        class Baker : Baker<CharacterRotationAuthoring>
        {
            public override void Bake(CharacterRotationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CharacterRotationComponent { 
                    Yaw = 0.0f,
                    MouseSensivity = authoring._mouseSensivity
                });
            }
        }
    }
}
