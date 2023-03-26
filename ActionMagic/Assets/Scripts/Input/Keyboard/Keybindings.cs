using System.Collections.Generic;

using UnityEngine;

using MagicEnv;

namespace InputEnv
{

    public class Keybindings : MonoBehaviour
    {
        private Dictionary<KeyCode, MagicType> _magicKeybindings = new Dictionary<KeyCode, MagicType>
        {
            {KeyCode.Alpha1, MagicType.Electro },
            {KeyCode.Alpha2, MagicType.Nature },
            {KeyCode.Alpha3, MagicType.Water }
        };

        public Dictionary<KeyCode, MagicType> MagicKeybindings => _magicKeybindings;       
    }
}
