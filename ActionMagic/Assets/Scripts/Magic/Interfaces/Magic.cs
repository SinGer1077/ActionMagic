using System;

using UnityEngine;

namespace MagicEnv
{
    [Serializable]
    public class Magic
    {     
        [SerializeField]
        private MagicType _magicType;
        public MagicType Type
        {
            get { return _magicType; }
        }
        
        [SerializeField]
        private ISpell[] _spells;
        public ISpell[] Spells
        {
            get { return _spells; }
        }       

        public Magic(GameObject spellGO)
        {
            InitSpells(spellGO);
        }

        public virtual void ShowSignal()
        {
            Debug.Log("Доступные спелы: ");
            foreach (ISpell spell in Spells)
            {
                Debug.Log(spell.Name);
            }
        }

        public void InitSpells(GameObject spellGO)
        {            
            _spells = spellGO.GetComponentsInChildren<ISpell>();
        }
    }
}
