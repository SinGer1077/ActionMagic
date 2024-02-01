using UnityEngine;
using Cinemachine;

using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;

using Character.Components;

namespace Character.Monobehaviour
{
    public class VirtualCameraFinder : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        private Entity _character;
        private bool _isNotNull;

        private void Start()
        {
            var characterQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(CharacterControllerComponent));
            NativeArray<Entity> entityNativeArray = characterQuery.ToEntityArray(Allocator.Temp);
            if (entityNativeArray.Length > 0)
            {
                _character = entityNativeArray[0];
                _isNotNull = true;
            }
        }

        private void LateUpdate()
        {
            if (_isNotNull)
            {
                LocalTransform characterTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(_character);
                _target.position = characterTransform.Position;
                _target.rotation = characterTransform.Rotation;
            }
        }
    }
}

