using System;

namespace ChaosHouse
{
    [Serializable]
    public class Face : CharacterPart
    {
        public Face()
        {
        }

        public Face(string nameValue, string[] materialNames) : base(nameValue, materialNames)
        {

        }
    }
}
