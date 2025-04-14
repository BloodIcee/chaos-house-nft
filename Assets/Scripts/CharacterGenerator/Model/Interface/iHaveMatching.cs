using System;
using UnityEngine;

namespace ChaosHouse
{
    [System.Serializable]
    public abstract class IHaveMatching : ScriptableObject
    {
        public abstract eCharacterGender Gender { get;}
    }
}
