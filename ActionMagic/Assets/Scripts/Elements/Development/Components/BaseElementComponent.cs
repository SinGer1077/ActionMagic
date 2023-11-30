using UnityEngine;
using Unity.Entities;

using Elements.Data;

namespace Elements.Components
{
    public struct BaseElementComponent : IComponentData
    {
        public ElementTypes Type;
    }
}
