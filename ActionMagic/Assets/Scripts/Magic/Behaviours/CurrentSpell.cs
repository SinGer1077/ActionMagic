using UnityEngine;

namespace MagicEnv {

    public class CurrentSpell : MonoBehaviour
    {
        [SerializeField]
        private CurrentMagic _magicContainer;

        public void CastSpell(int spellNumber)
        {
            if (_magicContainer.MagicInUse != null)
            {
                if (spellNumber < _magicContainer.MagicInUse.Spells.Length)
                {
                    _magicContainer.MagicInUse.Spells[spellNumber].Cast();
                }
            }
        }
    }
}
