using System;

namespace ChaosHouse
{
    [Serializable]
    public class Eyelashes : CharacterPart
    {
        public Eyelashes()
        {
        }

        public Eyelashes(string nameValue, string[] materialNames) : base(nameValue, materialNames)
        {

        }
    }
}
