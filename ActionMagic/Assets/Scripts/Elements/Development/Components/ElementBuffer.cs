using Unity.Entities;

using Elements.Data;

namespace Elements.Components
{
    public struct ElementBuffer : IBufferElementData
    {
        public ElementTypes type;
    }
}
