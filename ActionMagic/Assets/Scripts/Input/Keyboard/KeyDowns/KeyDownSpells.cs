using System.Collections.Generic;
using UnityEngine;
using MagicEnv;

namespace InputEnv
{

    public class KeyDownSpells : MonoBehaviour, IKeyDown
    {
        [SerializeField]
        private CurrentSpell _currentSpellController;

        private Dictionary<KeyCode, int> _keybindings = new Dictionary<KeyCode, int>
        {
            {KeyCode.Q, 0 },
            {KeyCode.W, 1 },
            {KeyCode.E, 2 }
        };
        public Dictionary<KeyCode, int> Keybindings { get; }

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
                    _currentSpellController.CastSpell((int)_keybindings[key]);
                }
            }
        }
    }
}
