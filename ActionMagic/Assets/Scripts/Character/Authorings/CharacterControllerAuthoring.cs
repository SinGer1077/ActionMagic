using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;

using Character.Components;

namespace Character.Authoring 
{
    public class CharacterControllerAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject _characterTransform;

        [SerializeField]
        private GameObject _spawnAttackPosition;

        class Baker : Baker<CharacterControllerAuthoring>
        {
            public override void Bake(CharacterControllerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CharacterControllerComponent
                {
                    CurrentType = 0,
                    CharacterParent = GetEntity(authoring._characterTransform, TransformUsageFlags.Dynamic),
                    SpawnAttackPosition = GetEntity(authoring._spawnAttackPosition, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}
