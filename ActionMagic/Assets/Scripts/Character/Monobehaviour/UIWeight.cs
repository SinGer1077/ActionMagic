using System.Collections.Generic;

using UnityEngine;
using Cinemachine;

using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

using Elements.Components;
using Universal.Components;

using TMPro;
public class UIWeight : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textPrefab;

    [SerializeField]
    private Transform _parent;

    private List<TextMeshProUGUI> _plashes; 
    
    // Start is called before the first frame update
    void Start()
    {
        _plashes = new List<TextMeshProUGUI>();
        for (int i = 0; i < 100; i++)
        {
            var gameObject = Instantiate(_textPrefab.gameObject, new Vector3(0.0f, -1000f, 0.0f), Quaternion.identity, _parent);
            _plashes.Add(gameObject.GetComponent<TextMeshProUGUI>());
        }
    }

    
    void FixedUpdate()
    {
        var baseElementQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(BaseElementComponent));
        NativeArray<Entity> entityNativeArray = baseElementQuery.ToEntityArray(Allocator.Temp);        

        for (int i = 0; i < entityNativeArray.Length; i++)
        {
            if (i >= _plashes.Count)
                break;

            var weight = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<WeightComponent>(entityNativeArray[i]);
            var localTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(entityNativeArray[i]);

            _plashes[i].text = weight.WeightValue.ToString();
            if (weight.Infinity)
            {
                _plashes[i].text += " &";
            }
            _plashes[i].transform.position = new float3(localTransform.Position.x, localTransform.Position.y + 1.0f, localTransform.Position.z);
        }
    }
}
