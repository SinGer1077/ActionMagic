using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elements {

    public class TriggerElement : MonoBehaviour
    {       
        private void OnTriggerEnter(Collider other)
        {
            EffectManager manager;
            if (other.gameObject.TryGetComponent(out manager))
            {
                Contact(manager);
            }
        }

        public virtual int GetIndex() { return 0; }

        private void Contact(EffectManager manager)
        {
            for (int i = 0; i < manager.Effects.Count; i++)
            {
                int reaction = ElementsCombos.GetReaction(GetIndex(), manager.Effects[i].Index);
                if (reaction == -1)
                {
                    continue;
                }

                Debug.Log("Reaction is " + reaction.ToString());
                break;
            }
        }
    }
}
