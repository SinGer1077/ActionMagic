using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputEnv {

    public class KeyDownsController : MonoBehaviour
    {      
        private IKeyDown[] _keyDowns;

        private void Start()
        {
            _keyDowns = GetComponents<IKeyDown>();
            foreach (IKeyDown key in _keyDowns)
            {
                Debug.Log(key.GetType());
            }
        }

        private void Update()
        {

        }
    }
}
