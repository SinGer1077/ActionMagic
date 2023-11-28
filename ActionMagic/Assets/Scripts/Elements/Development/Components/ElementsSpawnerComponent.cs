using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using Unity.Burst;

namespace Elements
{
    public struct ElementsSpawnerComponent : IComponentData
    {
        public NativeArray<ElementTypes> elements;        
    }
}
