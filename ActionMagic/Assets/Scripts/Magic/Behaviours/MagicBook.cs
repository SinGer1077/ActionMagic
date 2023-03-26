using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicEnv {

    public class MagicBook : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _magicGOs;

        private Dictionary<MagicType, Magic> _magics = new Dictionary<MagicType, Magic>
        {
            { MagicType.Electro, null },
            { MagicType.Water, null },
            { MagicType.Nature, null },           
        };

        public Dictionary<MagicType, Magic> Magics => _magics;

        private void Awake()
        {
            InitMagics();
        }

        private void InitMagics()
        {
            _magics[MagicType.Electro] = new ElectroMagic(_magicGOs[0]);
            _magics[MagicType.Nature] = new NatureMagic(_magicGOs[1]);
            _magics[MagicType.Water] = new WaterMagic(_magicGOs[2]);
        }
    }
}
