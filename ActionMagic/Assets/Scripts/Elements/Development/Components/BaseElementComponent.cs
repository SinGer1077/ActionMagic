using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Elements.Data;

namespace Elements.Components
{
    public struct BaseElementComponent : IComponentData
    {
        public int id;
        public ElementTypes Type;
    }
}
