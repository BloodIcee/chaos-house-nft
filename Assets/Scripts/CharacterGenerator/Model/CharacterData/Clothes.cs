using System;

namespace ChaosHouse
{
    [Serializable]
    public class Clothes : CharacterPart
    {
        public Clothes()
        {
        }

        public Clothes(string nameValue, string[] materialNames) : base(nameValue, materialNames)
        {
        }
    }
}