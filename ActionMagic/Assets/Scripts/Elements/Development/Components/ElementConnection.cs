using Unity.Collections;
using Unity.Entities;

using Elements.Components;

namespace Elements.Data
{
    public struct ElementConnection : IBufferElementData
    {
        public readonly int id;
        public readonly BaseElementComponent ConnectedElement;

        public ElementConnection (BaseElementComponent element)
        {
            id = element.id;
            ConnectedElement = element;
        }
    }
}
