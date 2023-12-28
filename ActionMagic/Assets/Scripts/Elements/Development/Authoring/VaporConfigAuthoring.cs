using UnityEngine;

using Unity.Entities;

using Elements.Components;

namespace Elements.Authoring
{
    public class VaporConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject vaporPrefab;

        class Baker : Baker<VaporConfigAuthoring>
        {
            public override void Bake(VaporConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new VaporConfigComponent
                {
                    VaporPrefab = GetEntity(authoring.vaporPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}
