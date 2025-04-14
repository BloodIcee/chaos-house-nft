using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "EyelashesOption", menuName = "UniqueCharacterOption/EyelashesOption")]
    public class EyelashesOption : UniqueCharacterOption
    {
        private List<Eyelashes> _eyelashesListInDatabase;
        private FemaleView _characterView;
        private eCharacterGender _gender;
#if UNITY_EDITOR
        public override int GetCountVariants()
        {
            if (_characterView == null) return 1;
            else return _characterView.GetAllEyelashesNames().Count;
        }

        public override void Init(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView, List<CharacterData> characterDatas)
        {
            if (gender == eCharacterGender.Male) return;

            _gender = gender;
            _characterView = characterView as FemaleView;
            _eyelashesListInDatabase = new List<Eyelashes>();
            for (int i = 0; i < characterDatas.Count; i++)
            {
                if (characterDatas[i].EyelashesData == null) continue;
                _eyelashesListInDatabase.Add(characterDatas[i].EyelashesData);
            }
        }

        public Eyelashes GetEyelashes(bool unique, int countVariants)
        {
            if (_gender == eCharacterGender.Male) return null;

            if (unique) return GetUniqueEyelashes(countVariants);
            else return GetRandomEyelashes();
        }

        private Eyelashes GetUniqueEyelashes(int countVariants)
        {
            var allEyelashesNames = _characterView.GetAllEyelashesNames();
            Eyelashes eyelashesName = new Eyelashes();

            for (int i = 0; i < allEyelashesNames.Count; i++)
            {
                var contains = _eyelashesListInDatabase.Find(x => x.PartName == allEyelashesNames[i]);
                if (contains == null)
                {
                    return new Eyelashes(allEyelashesNames[i], _characterView.GetEyelashesMainMaterialNames());
                }
            }

            int maxCountWithOneVariants = countVariants / allEyelashesNames.Count;
            int minValue = maxCountWithOneVariants;
            int index = -1;
            for (int i = 0; i < allEyelashesNames.Count; i++)
            {               
                var currentEyelashes = allEyelashesNames[i];
                var allVariantsWithCurrentEyelashes = _eyelashesListInDatabase.FindAll(x => x.PartName == currentEyelashes);

                if (allVariantsWithCurrentEyelashes != null)
                {
                    if (allVariantsWithCurrentEyelashes.Count < minValue)
                    {
                        minValue = allVariantsWithCurrentEyelashes.Count;
                        index = i;
                    }
                }
            }
            return new Eyelashes(allEyelashesNames[index], _characterView.GetActiveEyelashes().MaterialNames);
        }

        private Eyelashes GetRandomEyelashes()
        {
            var allEyelashesNames = _characterView.GetAllEyelashesNames();
            return new Eyelashes(allEyelashesNames[Random.Range(0, allEyelashesNames.Count)], _characterView.GetActiveEyelashes().MaterialNames);
        }
#endif
    }
}
