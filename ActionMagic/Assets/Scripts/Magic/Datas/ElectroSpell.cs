using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicEnv
{
    public class ElectroSpell : MonoBehaviour, Spell
    {
        private float _manaCost;
        public float ManaCost => _manaCost;

        private MagicType _type;
        public MagicType Type => _type;

        public void Cast() { }
    }
}
