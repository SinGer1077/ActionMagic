using Unity.Entities;

namespace Character.Components
{
    public struct CharacterRotationComponent : IComponentData
    {        
        public float Yaw;
        public float MouseSensivity;
    }
}
