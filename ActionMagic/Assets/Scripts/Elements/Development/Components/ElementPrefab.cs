using Unity.Entities;

using Elements.Data;

namespace Elements.Components
{
    public struct ElementPrefab : IBufferElementData
    {
        public ElementTypes type;
        public Entity Prefab;

        public ElementPrefab(ElementTypes _type, Entity prefab)
        {
            type = _type;
            Prefab = prefab;
        }
    }
}
