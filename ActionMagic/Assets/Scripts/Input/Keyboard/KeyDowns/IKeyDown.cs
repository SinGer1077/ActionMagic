using System.Collections.Generic;
using UnityEngine;

namespace InputEnv
{
    public interface IKeyDown
    {        
        bool IsBlocked { get; set; }
        void Update();        
    }
}
