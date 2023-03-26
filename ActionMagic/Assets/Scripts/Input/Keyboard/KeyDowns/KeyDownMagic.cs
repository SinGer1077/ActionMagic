using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicEnv;

namespace InputEnv
{

    public class KeyDownMagic : MonoBehaviour, IKeyDown
    {
        [SerializeField]
        private CurrentMagic _currentMagicController;

        private Dictionary<KeyCode, MagicType> _keybindings = new Dictionary<KeyCode, MagicType>
        {
            {KeyCode.Alpha1, MagicType.Electro },
            {KeyCode.Alpha2, MagicType.Nature },
            {KeyCode.Alpha3, MagicType.Water }
        };        
        public Dictionary<KeyCode, MagicType> Keybindings { get; }

        private bool _isBlocked;
        public bool IsBlocked
        {
            get { return _isBlocked; }
            set { _isBlocked = value; }
        }
        public void Update()
        {
            foreach (KeyCode key in _keybindings.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    _currentMagicController.SetCurrentMagic((int)_keybindings[key]);
                }
            }
        }
    }
}
