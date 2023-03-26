using UnityEngine;

namespace MagicEnv {

    public class CurrentSpell : MonoBehaviour
    {
        [SerializeField]
        private CurrentMagic _magicContainer;

        private ISpell _currentSpell;
        public ISpell SpellInUse => _currentSpell;

        public void CastSpell(int spellNumber)
        {
            if (_magicContainer.MagicInUse == null)
            {
                return;
            }

            if (spellNumber < _magicContainer.MagicInUse.Spells.Length)
            {
                if (_currentSpell != _magicContainer.MagicInUse.Spells[spellNumber] && _currentSpell != null)
                {
                    _currentSpell.Cancel();
                }

                _currentSpell = _magicContainer.MagicInUse.Spells[spellNumber];
                _currentSpell.Cast();
            }
        }
    }
}
