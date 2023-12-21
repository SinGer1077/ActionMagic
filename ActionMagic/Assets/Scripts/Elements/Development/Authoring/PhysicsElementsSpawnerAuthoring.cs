using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;

using Elements.Data;
using Elements.Components;

namespace Elements.Authoring
{
    public class PhysicsElementsSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField]
        private ElementTypes[] Types;

        [SerializeField]
        private GameObject[] TestPrefabs;

        class Baker : Baker<PhysicsElementsSpawnerAuthoring>
        {
            public override void Bake(PhysicsElementsSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);               

                var prefabBuffer = AddBuffer<ElementPrefab>(entity);
                for (int i = 0; i < authoring.TestPrefabs.Length; i++)
                {
                    prefabBuffer.Add(new ElementPrefab(
                        (ElementTypes)i, 
                        GetEntity(authoring.TestPrefabs[i], TransformUsageFlags.Dynamic))
                    );
                }

                var elementsArrayBuffer = AddBuffer<ElementBuffer>(entity);
                for (int i = 0; i < authoring.Types.Length; i++)
                {
                    elementsArrayBuffer.Add(new ElementBuffer { type = authoring.Types[i] });
                }

                AddComponent(entity, new PhysicsElementsSpawnerComponent
                {
                    Spawner = entity
                });
            }
        }
    }
}
