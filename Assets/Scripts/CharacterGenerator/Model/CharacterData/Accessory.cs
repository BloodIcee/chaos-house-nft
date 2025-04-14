using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [System.Serializable]
    public class Accessory : CharacterPart
    {
        public Accessory()
        {
        }

        public Accessory(string nameValue, string[] materialNames) : base(nameValue, materialNames)
        {
        }
    }
}
