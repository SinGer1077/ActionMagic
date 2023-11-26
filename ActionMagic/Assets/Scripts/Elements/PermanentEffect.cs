using System.Linq;

using UnityEngine;
using Elements;

public class PermanentEffect : MonoBehaviour
{
    [SerializeField]
    private EffectEnum _effect;

    private EffectManager _manager;

    private void Awake()
    {
        _manager = GetComponent<EffectManager>();
        SetEffect();
    }

    private void SetEffect()
    {
        var effectInList = _manager.Effects.Find(x => x.Index == (int)_effect);
        if (effectInList == null)
        {
            _manager.AddEffect(new Effect((int)_effect));
        }
    }
}
