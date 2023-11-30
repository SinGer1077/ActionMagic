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
        private ElementTypes[] elements;

        class Baker : Baker<ElementsSpawnerAuthoring>
        {
            public override void Bake(ElementsSpawnerAuthoring authoring)
            {
                NativeArray<ElementTypes> array = new NativeArray<ElementTypes>(authoring.elements, Allocator.Temp);                
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ElementsSpawnerComponent
                {
                    elements = array
                });
            }
        }

    }
}
