using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "AccessoryOption", menuName = "UniqueCharacterOption/AccessoryOption")]
    public class AccessoryOption : UniqueCharacterOption
    {
        private MatchingAccessoriesMaterials _matchingAccessoriesMaterials;
        private List<Accessory> _accessoryListInDatabase;
        private CharacterView _characterView;
#if UNITY_EDITOR
        public override int GetCountVariants()
        {
            return GetAllAccessoriesVariants().Count;
            
        }

        public override void Init(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView, List<CharacterData> characterDatas)
        {
            _matchingAccessoriesMaterials = matchingDatabase.GetMatchingConfig<MatchingAccessoriesMaterials>(gender);
            _accessoryListInDatabase = new List<Accessory>();
            _characterView = characterView;

            for (int i = 0; i < characterDatas.Count; i++)
            {
                _accessoryListInDatabase.Add(characterDatas[i].Accessory);
            }
        }

        public Accessory GetAccessory(bool unique, int countVariants)
        {
            if (unique) return GetUniqueAccessory(countVariants);
            else return GetRandomAccessory();
        }

        private List<Accessory> GetAllAccessoriesVariants()
        {
            List<Accessory> accessories = new List<Accessory>();

            var allAccessoryNames = _characterView.GetAllAccessoriesNames();
            var allMaterialNames = _matchingAccessoriesMaterials.GetMaterialNames();
           
            for (int i = 0; i < allAccessoryNames.Count; i++)
            {
                
                for (int j = 0; j < allMaterialNames.Count; j++)
                {
                    if (accessories.Find(x => x.PartName == "None") != null && allAccessoryNames[i] == "None") continue;
                    accessories.Add(new Accessory(allAccessoryNames[i], new string[] { allMaterialNames[j] }));
                }
            }

            return accessories;
        } 
        private Accessory GetUniqueAccessory(int countVariants)
        {
            var allAccessories = GetAllAccessoriesVariants();

            for (int i = 0; i < allAccessories.Count; i++)
            {
                var contains = _accessoryListInDatabase.Find(x => x.PartName == allAccessories[i].PartName && x.NameMaterialsIsMatch(allAccessories[i].MaterialNames.ToList()));
                if (contains == null)
                {
                    return allAccessories[i];
                }
            }

            int maxCountWithOneVariants = countVariants / allAccessories.Count;
            int minValue = maxCountWithOneVariants;
            int index = -1;
            for (int i = 0; i < allAccessories.Count; i++)
            {
                var currentAccessory = allAccessories[i];
                var allVariantsWithCurrentAccessory = _accessoryListInDatabase.FindAll(x => x.PartName == currentAccessory.PartName && x.NameMaterialsIsMatch(allAccessories[i].MaterialNames.ToList()));

                if (allVariantsWithCurrentAccessory != null)
                {
                    if (allVariantsWithCurrentAccessory.Count < minValue)
                    {
                        minValue = allVariantsWithCurrentAccessory.Count;
                        index = i;
                    }
                }
            }

            return allAccessories[index];
        }

        private Accessory GetRandomAccessory()
        {
            var allAccessories = GetAllAccessoriesVariants();
            return allAccessories[Random.Range(0, allAccessories.Count)];
            
        }
#endif
    }
}
