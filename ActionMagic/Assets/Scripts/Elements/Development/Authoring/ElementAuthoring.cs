using Unity.Entities;

using UnityEngine;

using Elements.Components;
using Elements.Data;
using Universal.Components;

namespace Elements.Authoring
{
    public class ElementAuthoring : MonoBehaviour
    {
        [SerializeField]
        private ElementTypes type;
        [SerializeField]
        private float weight;
        [SerializeField]
        private bool infinity;

        class Baker : Baker<ElementAuthoring>
        {
            public override void Bake(ElementAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new BaseElementComponent
                {
                    id = entity,
                    Type = authoring.type
                });

                AddComponent(entity, new WeightComponent
                {
                    WeightValue = authoring.weight,
                    Infinity = authoring.infinity,
                    InitWeightValue = authoring.weight
                });
                
                AddBuffer<ElementConnection>(entity);
            }
        }
    }
}
