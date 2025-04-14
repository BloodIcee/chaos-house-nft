using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public abstract class UniqueCharacterOption: ScriptableObject
    {
#if UNITY_EDITOR
        public abstract int GetCountVariants();
        public abstract void Init(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView, List<CharacterData> characterDatas);
#endif
    }
}
