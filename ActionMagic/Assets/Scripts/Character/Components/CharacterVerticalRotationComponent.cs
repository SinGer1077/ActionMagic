using Unity.Entities;
using Unity.Mathematics;

namespace Character.Components
{
    public struct CharacterVerticalRotationComponent : IComponentData
    {
        public float Pitch;        
        public float MouseSensivity;
        public float2 AngleBorders;
    }
}

