using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "BodyOption", menuName = "UniqueCharacterOption/BodyOption")]
    public class BodyOption : UniqueCharacterOption
    {
        private CharacterView _characterView;
        private List<Body> _bodyListInDatabase;
        private MatchingBodySkinAndMaterials _matchingBodySkinAndMaterials;
#if UNITY_EDITOR
        public override int GetCountVariants()
        {
            return _matchingBodySkinAndMaterials.GetTextureNames().Count;
        }

        public override void Init(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView, List<CharacterData> characterDatas)
        {
            _characterView = characterView;

            _matchingBodySkinAndMaterials = matchingDatabase.GetMatchingConfig<MatchingBodySkinAndMaterials>();
            
            _bodyListInDatabase = new List<Body>();
            for (int i = 0; i < characterDatas.Count; i++)
            {
                _bodyListInDatabase.Add(characterDatas[i].BodyData);
            }
            
        }

        public Body GetBody(bool unique, int countVariants)
        {
            if (unique) return GetUniqueBody(countVariants);
            else return _matchingBodySkinAndMaterials.GetRandomBody(_characterView.GetBody().PartName, _characterView.GetBody().MaterialNames);
        }

        private Body GetUniqueBody(int countVariants)
        {
            return _matchingBodySkinAndMaterials.GetUniqueBody(_bodyListInDatabase, _characterView.GetBody().PartName, _characterView.GetBody().MaterialNames, countVariants);
        }
#endif
    }
}
