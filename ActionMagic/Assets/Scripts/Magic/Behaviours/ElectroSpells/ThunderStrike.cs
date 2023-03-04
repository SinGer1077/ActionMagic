using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicEnv
{
    public class ThunderStrike : ElectroSpell
    {
        public void Start()
        {
            Cast();
        }

        public new void Cast()
        {
            Debug.Log("Бум шакалака");
        }
    }
}
