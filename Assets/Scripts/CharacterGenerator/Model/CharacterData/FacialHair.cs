using System;

namespace ChaosHouse
{
    [Serializable]
    public class FacialHair : CharacterPart
    {

        public FacialHair()
        {
        }

        public FacialHair(string nameValue, string[] materialNames) : base(nameValue, materialNames)
        {
           
        }
    }
}