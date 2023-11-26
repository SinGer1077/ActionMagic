using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elements
{
    public static class ElementsCombos
    {
        private static int[,] _elementsComboMatrix = new int[,]
            //0  1  2  3  4  5  6  7  8  9 10 11
        {
      /*0*/{ -1, 12,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
      /*1*/{  12,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
      /*2*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
      /*3*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
      /*4*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
      /*5*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
      /*6*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
      /*7*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
      /*8*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
      /*9*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
     /*10*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
     /*11*/{ -1,-1,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1 },
        };    
        
        public static int GetReaction(int trigger, int effect)
        {
            return _elementsComboMatrix[trigger, effect];
        }
       
    }
}
