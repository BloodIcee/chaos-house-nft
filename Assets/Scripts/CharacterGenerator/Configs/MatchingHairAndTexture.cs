using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "MatchingHairAndTexture", menuName = "Configs/MatchingHairAndTexture")]
    public class MatchingHairAndTexture : IHaveMatching
    {
        [SerializeField] private eCharacterGender _gender;
        [SerializeField] private List<AssetReference> textures = new List<AssetReference>();
        public override eCharacterGender Gender => _gender;

#if UNITY_EDITOR
        public List<string> GetTextureNames()
        {

            List<string> textureNamesList = new List<string>();
            for (int i = 0; i < textures.Count; i++)
            {
                textureNamesList.Add(textures[i].editorAsset.name);
            }

            return textureNamesList;
        }

        public Hair GetHairWithUniqueTexture(List<Hair> exclusionList, string hairName, string[] materialNames)
        {
            Hair hair = new Hair();

            var allHairTextureVariants = GetTextureNames();

            for (int i = 0; i < allHairTextureVariants.Count; i++)
            {
                var currentHairTexture = allHairTextureVariants[i];
                var containts = exclusionList.Find(x => x.TextureIsMatch(currentHairTexture));
                if (containts == null) return new Hair(hairName, materialNames, currentHairTexture);
            }

            hair = new Hair(hairName, materialNames, allHairTextureVariants[Random.Range(0, allHairTextureVariants.Count)]);

            return hair;
        }
#endif
    } 
}

