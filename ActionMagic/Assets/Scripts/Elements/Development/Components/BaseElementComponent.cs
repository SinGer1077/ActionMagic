using UnityEngine;
using Unity.Entities;

namespace Elements
{
    public struct BaseElementComponent : IComponentData
    {
        public ElementTypes Type;
    }
}
