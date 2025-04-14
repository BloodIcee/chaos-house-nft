using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "MatchingBodySkinAndMaterials", menuName = "Configs/MatchingBodySkinAndMaterials")]
    public class MatchingBodySkinAndMaterials : IHaveMatching
    {
        [SerializeField] private eCharacterGender _gender;
        [SerializeField] private List<AssetReference> textures = new List<AssetReference>();
        public override eCharacterGender Gender => _gender;



#if UNITY_EDITOR

        public Body GetUniqueBody(List<Body> exclusionList, string bodyName, string [] materialNames, int countVariants)
        {
            Body body = new Body();

            var allBodyTextureVariants = GetTextureNames();

            for (int i = 0; i < allBodyTextureVariants.Count; i++)
            {
                var currentBodyTexture = allBodyTextureVariants[i];
                var containts = exclusionList.Find(x => x.TextureNameIsMatch(currentBodyTexture));
                if (containts == null) return new Body(bodyName, materialNames, currentBodyTexture);
            }

            int maxCountWithOneTexture = countVariants / allBodyTextureVariants.Count;
            int minCount = maxCountWithOneTexture;
            int index = -1;

            for (int i = 0; i < allBodyTextureVariants.Count; i++)
            {
                var currentTexture= allBodyTextureVariants[i];
                var allVariantsWithCurrentTexture = exclusionList.FindAll(x => x.TextureName == currentTexture);
                if (allVariantsWithCurrentTexture != null)
                {
                    if (allVariantsWithCurrentTexture.Count < minCount)
                    {
                        minCount = allVariantsWithCurrentTexture.Count;
                        index = i;
                    }
                }
            }

            body = new Body(bodyName, materialNames, allBodyTextureVariants[index]);

            return body;
        }

        public Body GetRandomBody(string bodyName, string[] materialNames)
        {
            var allBodyTextureVariants = GetTextureNames();
            return new Body(bodyName, materialNames, allBodyTextureVariants[Random.Range(0, allBodyTextureVariants.Count)]);
        }
        public List<string> GetTextureNames()
        {

            List<string> textureNamesList = new List<string>();
            for (int i = 0; i < textures.Count; i++)
            {
                textureNamesList.Add(textures[i].editorAsset.name);
            }

            return textureNamesList;
        }
#endif
    }

}
