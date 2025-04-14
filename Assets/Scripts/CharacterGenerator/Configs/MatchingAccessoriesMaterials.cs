using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "MatchingAccessoriesMaterials", menuName = "Configs/MatchingAccessoriesMaterials")]
    public class MatchingAccessoriesMaterials : IHaveMatching
    {
        [SerializeField] private eCharacterGender _gender;
        [SerializeField] private List<AssetReference> _materials = new List<AssetReference>();
        public override eCharacterGender Gender => _gender;


#if UNITY_EDITOR
        public string GetUniqueAccessoriesMaterialName(List<Accessory> exclusionList)
        {
            
            var allMaterialsNames = GetMaterialNames();
            string materialName = allMaterialsNames[Random.Range(0, _materials.Count)];

            for (int i = 0; i < allMaterialsNames.Count; i++)
            {
                var currentAccessoryMaterial = allMaterialsNames[i];
                var containts = exclusionList.Find(x => x.NameMaterialsIsMatch(new List<string> { currentAccessoryMaterial }));
                if (containts == null) return currentAccessoryMaterial;
            }            

            return materialName;
        }
        public List<string> GetMaterialNames()
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
