using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using Unity.Burst;

using Elements.Data;

namespace Elements.Components
{
    public struct ElementsSpawnerComponent : IComponentData
    {
        public int Count;
    }
}
