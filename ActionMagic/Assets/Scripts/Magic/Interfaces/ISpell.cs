using System;

namespace MagicEnv
{    
    public interface ISpell
    {
        public float ManaCost {get;}

        public string Name { get; }
       
        public MagicType Type { get; }

        public void Cast();

        public void Cancel();


        
    }
}
