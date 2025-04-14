using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "ClothesOption", menuName = "UniqueCharacterOption/ClothesOption")]
    public class ClothesOption : UniqueCharacterOption
    {
        private MatchingClothingAndMaterials _matchingClothing;
        private List<Clothes> _clothesListInDatabase;
        private bool isInited;
#if UNITY_EDITOR
        public override int GetCountVariants()
        {           
            return _matchingClothing.GetAllClothesVariant().Count;
        }

        public override void Init(eCharacterGender gender, MatchingDatabase matchingData, CharacterView characterView, List<CharacterData> characterDatas)
        {
            if (isInited) return;

            isInited = true;
            _matchingClothing = matchingData.GetMatchingConfig<MatchingClothingAndMaterials>(gender);
            _clothesListInDatabase = new List<Clothes>();

            for (int i = 0; i < characterDatas.Count; i++)
            {
                _clothesListInDatabase.Add(characterDatas[i].Clothes);
            }
        }

        public Clothes GetClothes(bool unique, int countVariants)
        {
            if (unique) return GetUniqueClothes(countVariants);
            else return _matchingClothing.GetRandomClothes();
        }
        private Clothes GetUniqueClothes(int countVariants)
        {
            return _matchingClothing.GetUniqueClothes(_clothesListInDatabase, countVariants);
        }
#endif
    }
}
