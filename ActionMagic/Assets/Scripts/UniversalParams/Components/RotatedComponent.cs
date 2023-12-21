using Unity.Entities;
using UnityEngine;

namespace Universal.Components
{
    public struct RotatedComponent : IComponentData
    {
        public float RotationSpeed;
        public Vector3 RotationVector;
    }
}
