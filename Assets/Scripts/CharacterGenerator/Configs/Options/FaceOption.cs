using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "FaceOption", menuName = "UniqueCharacterOption/FaceOption")]
    public class FaceOption : UniqueCharacterOption
    {
#if UNITY_EDITOR
        private CharacterView _characterView;
        public override int GetCountVariants()
        {
            return 1;
        }

        public override void Init(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView, List<CharacterData> characterDatas)
        {
            _characterView = characterView;
        }

        public Face GetFace()
        {
            return _characterView.GetFace();
        }
#endif
    }
}
