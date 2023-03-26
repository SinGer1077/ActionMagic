using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicEnv {

    public class CurrentMagic : MonoBehaviour
    {
        [SerializeField]
        private MagicBook _magicBook;

        [SerializeField]
        private CurrentSpell _spellController;

        private Magic _currentMagic;
        public Magic MagicInUse => _currentMagic;

        public void SetCurrentMagic(int newMagic)
        {
            if (_currentMagic != null && _currentMagic.Type != (MagicType)newMagic && _spellController.SpellInUse != null)
            {
                _spellController.SpellInUse.Cancel();
            }

            switch ((MagicType)newMagic)
            {
                case MagicType.Electro:
                    _currentMagic = _magicBook.Magics[MagicType.Electro];
                    break;
                case MagicType.Nature:
                    _currentMagic = _magicBook.Magics[MagicType.Nature];                   
                    break;
                case MagicType.Water:
                    _currentMagic = _magicBook.Magics[MagicType.Water];                    
                    break;
                default:
                    Debug.Log("��� ����");
                    _currentMagic = null;
                    break;

            }

            if (_currentMagic != null)
            {
                _currentMagic.ShowSignal();
            }
        }
    }
}
