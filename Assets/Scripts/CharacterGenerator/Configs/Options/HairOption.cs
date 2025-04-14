using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "HairOption", menuName = "UniqueCharacterOption/HairOption")]
    public class HairOption : UniqueCharacterOption
    {
        private MatchingHairAndTexture _matchingHairAndTexture;
        private CharacterView _characterView;
        private List<Hair> _hairListInDatabase;
#if UNITY_EDITOR
        public override int GetCountVariants()
        {
            return GetAllHairVariant().Count;
        }

        public override void Init(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView, List<CharacterData> characterDatas)
        {
            _matchingHairAndTexture = matchingDatabase.GetMatchingConfig<MatchingHairAndTexture>();
            _characterView = characterView;

            _hairListInDatabase = new List<Hair>();
            for (int i = 0; i < characterDatas.Count; i++)
            {
                _hairListInDatabase.Add(characterDatas[i].HairData);
            }
        }
        private List<Hair> GetAllHairVariant()
        {
            List<Hair> hairList = new List<Hair>();

            List<string> allHairNames = _characterView.GetAllHairNames();
            var allTexute = _matchingHairAndTexture.GetTextureNames();

            for (int i = 0; i < allHairNames.Count; i++)
            {
                for (int j = 0; j < allTexute.Count; j++)
                {
                    Hair hair = new Hair(allHairNames[i], _characterView.GetCurrentHair.MaterialNames, allTexute[j]);
                    hairList.Add(hair);
                }
            }
            return hairList;
        }
        public Hair GetHair(bool unique, int countVariants)
        {
            if (unique) return GetUniqueHair(countVariants);
            else return GetRandomHair();
        }

        private Hair GetRandomHair()
        {
            List<Hair> allHair = GetAllHairVariant();
            return allHair[Random.Range(0, allHair.Count)];

        }

        private Hair GetUniqueHair(int countVariants)
        {
            List<Hair> allHair = GetAllHairVariant();

            Hair hair = new Hair();

            for (int i = 0; i < allHair.Count; i++)
            {
                var contains = _hairListInDatabase.Find(x => x.PartName == allHair[i].PartName && x.TextureName == allHair[i].TextureName);
                if (contains == null)
                {
                    return allHair[i];                    
                }
            }

            int maxCountWithOneVariants = countVariants / allHair.Count;
            int minValue = maxCountWithOneVariants;
            int index = -1;
            for (int i = 0; i < allHair.Count; i++)
            {
                var currentHair = allHair[i];
                var allVariantsWithCurrentHair = _hairListInDatabase.FindAll(x=>x.PartName == currentHair.PartName && x.TextureName == currentHair.TextureName);

                if (allVariantsWithCurrentHair != null)
                {
                    if (allVariantsWithCurrentHair.Count < minValue)
                    {
                        minValue = allVariantsWithCurrentHair.Count;
                        index = i;
                    }
                }
            }

             return allHair[index];
        }

#endif
    }
}
