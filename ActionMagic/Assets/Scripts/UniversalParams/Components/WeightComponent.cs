using Unity.Entities;

namespace Universal.Components
{
    public struct WeightComponent : IComponentData
    {
        public float WeightValue;
        public bool Infinity;

        public float InitWeightValue;
        public float LerpTimer;
    }
}
