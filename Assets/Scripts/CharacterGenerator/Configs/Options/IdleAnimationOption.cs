using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "IdleAnimationOption", menuName = "UniqueCharacterOption/IdleAnimationOption")]
    public class IdleAnimationOption : UniqueCharacterOption
    {
        private IdleAnimationConfig _idleAnimationConfig;
        private List<string> _animationListInDatabase;
#if UNITY_EDITOR
        public override int GetCountVariants()
        {
           return _idleAnimationConfig.GetAnimationNames().Count;
        }


        public string GetIdleAnimationName(bool unique, int countVariant)
        {
            if (unique) return GetUniqueIdleAnimationName( countVariant);
            else return GetRandomIdleAnimationName();
        } 
        private string GetRandomIdleAnimationName()
        {
            var allAnimationsName = _idleAnimationConfig.GetAnimationNames();
            string animationName = allAnimationsName[Random.Range(0, allAnimationsName.Count)];
            return animationName;
        }
        private string GetUniqueIdleAnimationName( int countVariant)
        {
            return _idleAnimationConfig.GetUniqueIdleAnimationName(_animationListInDatabase, countVariant);                        
        }

        public override void Init(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView, List<CharacterData> characterDatas)
        {
            _idleAnimationConfig = matchingDatabase.GetMatchingConfig<IdleAnimationConfig>();
            _animationListInDatabase = new List<string>();

            for (int i = 0; i < characterDatas.Count; i++)
            {
                _animationListInDatabase.Add(characterDatas[i].IdleAnimationName);
            }
        }
#endif
    }
}
