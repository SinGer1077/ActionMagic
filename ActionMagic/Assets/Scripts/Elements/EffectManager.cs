using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elements
{

    public class EffectManager : MonoBehaviour
    {
        private List<Effect> _effects = new List<Effect>();

        public List<Effect> Effects => _effects;

        public void AddEffect(Effect newEffect)
        {
            _effects.Add(newEffect);
        }
    }
}
