using UnityEngine;

using Unity.Entities;

namespace Character.Authoring
{
    public class CameraAuthoring : MonoBehaviour
    {
        class Baker : Baker<CameraAuthoring>
        {
            public override void Bake(CameraAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
            }
        }
    }
}
