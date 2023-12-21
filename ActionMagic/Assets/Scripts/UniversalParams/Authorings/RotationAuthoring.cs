using UnityEngine;

using Unity.Entities;
using Unity.Transforms;

using Universal.Components;

namespace Universal.Authorings {
    public class RotationAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed;
        
        [SerializeField]
        private Vector3 rotationVector;

        class Baker : Baker<RotationAuthoring>
        {
            public override void Bake(RotationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new RotatedComponent
                {
                    RotationSpeed = authoring.rotationSpeed,
                    RotationVector = authoring.rotationVector
                });              
            }
        }
    }
}
