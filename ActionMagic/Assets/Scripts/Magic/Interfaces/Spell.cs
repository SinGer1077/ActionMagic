using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicEnv
{
    public interface Spell
    {
        public float ManaCost {get;}
       
        public MagicType Type { get; }

        public void Cast();
    }
}
