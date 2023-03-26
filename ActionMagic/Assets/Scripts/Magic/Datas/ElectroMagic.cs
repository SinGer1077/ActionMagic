using UnityEngine;

namespace MagicEnv
{
    public class ElectroMagic : Magic
    {
        public ElectroMagic(GameObject spellGO) : base(spellGO)
        {

        }

        public override void ShowSignal()
        {           
            Debug.Log("Активировали электро магию");
            base.ShowSignal();
        }
    }
}
