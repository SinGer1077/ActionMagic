using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicEnv
{
    public class NatureMagic : Magic
    {
        public NatureMagic(GameObject spellGO) : base(spellGO)
        {

        }

        public override void ShowSignal()
        {
            Debug.Log("Активировали магию природы");
            base.ShowSignal();
        }
    }
}