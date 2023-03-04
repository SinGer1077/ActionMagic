using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicEnv
{
    public class Magic
    {     
        [SerializeField]
        private MagicType _magicType;

        public MagicType Type
        {
            get { return _magicType; }
        }

        private Spell[] _spells;

        public Spell[] Spells
        {
            get { return _spells; }
        }
    }
}
