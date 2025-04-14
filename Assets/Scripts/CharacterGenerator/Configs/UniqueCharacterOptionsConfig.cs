using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "UniqueCharacterOptionsConfig", menuName = "Configs/UniqueCharacterOptionsConfig")]
    public class UniqueCharacterOptionsConfig : ScriptableObject
    {
        [field: SerializeField] public List<UniqueCharacterOption> UniqueOptionsList { get; private set; }

        public bool Contains<T>(T uniqueCharacter)where T : UniqueCharacterOption
        {
            for (int i = 0; i < UniqueOptionsList.Count; i++)
            {
                if (UniqueOptionsList[i] is T) {
                    return true;
                }
            }

            return false;
        }
    }
}
