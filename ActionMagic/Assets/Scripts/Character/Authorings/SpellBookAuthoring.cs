using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities;

using Character.Data;
using Character.Components;

namespace Character.Authoring
{
    public class SpellBookAuthoring : MonoBehaviour
    {
        [SerializeField]
        private SimpleProjectileConfig[] _spells;

        class Baker : Baker<SpellBookAuthoring>
        {
            public override void Bake(SpellBookAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new MagicBookComponent {SpellBook = entity});
                var buffer = AddBuffer<SpellBuffer>(entity);
                for (int i = 0; i < authoring._spells.Length; i++)
                {
                    buffer.Add(new SpellBuffer
                    {
                        SpellPrefab = GetEntity(authoring._spells[i].Spell, TransformUsageFlags.None),
                        FlySpeed = authoring._spells[i].FlySpeed,
                        ElementWeight = authoring._spells[i].Weight,
                        LifeTime = authoring._spells[i].Weight
                    });
                }
            }
        }
    }
}
