using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "MatchingClothingAndMaterials", menuName = "Configs/MatchingClothingAndMaterials")]
    public class MatchingClothingAndMaterials : IHaveMatching
    {
        [SerializeField] private eCharacterGender _gender;             
        [SerializeField] private CharacterView _characterPfb;
        [SerializeField] private List<ClothMaterialMatchingData> _clothMaterialMatching = new List<ClothMaterialMatchingData>();

        private void Reset()
        {
            _clothMaterialMatching.Clear();
        }

        public override eCharacterGender Gender => _gender;
        public CharacterView CharacterPfb => _characterPfb;

#if UNITY_EDITOR
        public Clothes GetRandomClothes()
        {
            var allClothesVariants = GetAllClothesVariant();
            return allClothesVariants[Random.Range(0, allClothesVariants.Count)];
        }

        public List<Clothes> GetAllClothesVariant( )
        {
            List<Clothes> clothesList = new List<Clothes>();            
            
            for (int i = 0; i < _clothMaterialMatching.Count; i++)
            {
                for (int j = 0; j < _clothMaterialMatching[i].Materials.Count; j++)
                {
                    Clothes clothes = new Clothes(_clothMaterialMatching[i].ClothesName, new string[] { _clothMaterialMatching[i].GetMaterialsNames()[j] });
                    clothesList.Add(clothes);
                }                
            }            
            return clothesList;
        }

        public Clothes GetUniqueClothes(List<Clothes> exclusionList, int countVariants)
        {
            Clothes clothes = new Clothes();

            var allClothesVariants = GetAllClothesVariant();

            for (int i = 0; i < allClothesVariants.Count; i++)
            {
                var currentClothes = allClothesVariants[i];
                var containts = exclusionList.Find(x => x.PartName == currentClothes.PartName && x.NameMaterialsIsMatch(currentClothes.MaterialNames.ToList()));
                if (containts == null) return currentClothes;
            }

            if (clothes.PartName == "")
            {
                var maxCountWithOneClothes = countVariants / allClothesVariants.Count;
                int minCount = maxCountWithOneClothes;
                int index = -1;
                for (int i = 0; i < allClothesVariants.Count; i++)
                {
                    var currentClothes = allClothesVariants[i];
                    var clothesVariants = exclusionList.FindAll(x => x.PartName == currentClothes.PartName && x.NameMaterialsIsMatch(currentClothes.MaterialNames.ToList()));
                    if (clothesVariants != null)
                    {
                        if (clothesVariants.Count < minCount)
                        {
                            minCount = clothesVariants.Count;
                            index = i;
                        }
                    }
                }
                clothes = allClothesVariants[index];
            }            

            return clothes;
        }
        public List<string> GetMaterialNames(string clothesName)
        {
            var matching = _clothMaterialMatching.Find(x=>x.ClothesName == clothesName);

            if (matching != null)
            {
                List<string> materilNames = new List<string>();
                for (int i = 0; i < matching.Materials.Count; i++)
                {
                    materilNames.Add(matching.Materials[i].editorAsset.name);
                }
                return materilNames;
            }

            return null;
        }
#endif

    }

    [System.Serializable]
    public class ClothMaterialMatchingData 
    {
        [SerializeField] private string _clothName;
        [SerializeField] private List<AssetReference> _materials;

        public string ClothesName => _clothName;
        public List<AssetReference> Materials => _materials;

#if UNITY_EDITOR         
   
        public List<string> GetMaterialsNames()
        {
            List<string> materilNames = new List<string>();
            for (int i = 0; i < _materials.Count; i++)
            {
                materilNames.Add(_materials[i].editorAsset.name);
            }
            return materilNames;
        }
#endif
    }

}
