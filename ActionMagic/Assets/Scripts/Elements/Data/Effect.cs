using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elements
{
    public class Effect
    {
        private int _index;
        public int Index => _index;

        public Effect(int index)
        {
            _index = index;
        }
    }
}
