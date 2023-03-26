using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicEnv
{
    public class WaterMagic : Magic
    {            
        public WaterMagic(GameObject spellGO) : base (spellGO)
        {
            
        }

        public override void ShowSignal()
        {
            Debug.Log("Активировали магию воды");
            base.ShowSignal();
        }
    }
}

