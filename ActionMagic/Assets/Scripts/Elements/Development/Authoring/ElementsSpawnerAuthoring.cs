using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;

using Elements.Data;
using Elements.Components;

namespace Elements.Authoring
{

    public class ElementsSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField]
        private int Count;

        class Baker : Baker<ElementsSpawnerAuthoring>
        {            
            public override void Bake(ElementsSpawnerAuthoring authoring)
            {               
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ElementsSpawnerComponent
                {
                    Count = authoring.Count
                });
            }
        }

    }
}
