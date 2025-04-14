using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace  ChaosHouse
{
[CreateAssetMenu(fileName = "FacialHairOption", menuName = "UniqueCharacterOption/FacialHairOption")]
    public class FacialHairOption : UniqueCharacterOption
    {
        private List<FacialHair> _facialHairListInDatabase;
        private MaleView _characterView;
#if UNITY_EDITOR
        public override int GetCountVariants()
        {
            if (_characterView == null) return 1;
            else return GetAllFacialHairVariants().Count;

        }

        public override void Init(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView, List<CharacterData> characterDatas)
        {
            _facialHairListInDatabase = new List<FacialHair>();
            _characterView = characterView as MaleView;

            for (int i = 0; i < characterDatas.Count; i++)
            {
                if(characterDatas[i].FacialHairData!=null) _facialHairListInDatabase.Add(characterDatas[i].FacialHairData);
            }
        }

        public FacialHair GetFacialHair(bool unique, int countVariants)
        {
            if (unique) return GetUniqueGetFacialHair(countVariants);
            else return GetRandomGetFacialHair();
        }

        private List<FacialHair> GetAllFacialHairVariants()
        {
            
            List<FacialHair> facialHairs = new List<FacialHair>();
            if (_characterView == null) return facialHairs;

              var allfacialHairs = _characterView.GetAllFacialHairNames();
            var facialHairMatrerials = _characterView.GetFacialHairMainMaterialNames();
            for (int i = 0; i < allfacialHairs.Count; i++)
            {
                if (allfacialHairs[i] == "None") continue;
                facialHairs.Add(new FacialHair(allfacialHairs[i], facialHairMatrerials));
            }

            return facialHairs;
        }
        private FacialHair GetUniqueGetFacialHair(int countVariants)
        {
            var allFacialHairs = GetAllFacialHairVariants();
            if (_characterView == null) return null;
            if(_facialHairListInDatabase.Count == 0) return allFacialHairs[0];

            for (int i = 0; i < allFacialHairs.Count; i++)
            {
                var contains = _facialHairListInDatabase.Find(x => x.PartName == allFacialHairs[i].PartName && x.NameMaterialsIsMatch(allFacialHairs[i].MaterialNames.ToList()));
                if (contains == null)
                {
                    return allFacialHairs[i];
                }
            }

            int maxCountWithOneVariants = countVariants / allFacialHairs.Count;
            int minValue = maxCountWithOneVariants;
            int index = -1;
            for (int i = 0; i < allFacialHairs.Count; i++)
            {
                var currentFacialHairs = allFacialHairs[i];
                var allVariantsWithCurrentFH = _facialHairListInDatabase.FindAll(x => x.PartName == currentFacialHairs.PartName && x.NameMaterialsIsMatch(allFacialHairs[i].MaterialNames.ToList()));

                if (allVariantsWithCurrentFH != null)
                {
                    if (allVariantsWithCurrentFH.Count < minValue)
                    {
                        minValue = allVariantsWithCurrentFH.Count;
                        index = i;
                    }
                }
            }

            return allFacialHairs[index];
        }

        private FacialHair GetRandomGetFacialHair()
        {
            var allFacialHairs = GetAllFacialHairVariants();
            return allFacialHairs[Random.Range(0, allFacialHairs.Count)];

        }
#endif
    }
}