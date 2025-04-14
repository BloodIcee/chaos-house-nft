using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "IdleAnimationConfig", menuName = "Configs/IdleAnimationConfig")]
    public class IdleAnimationConfig : IHaveMatching
    {
       [SerializeField] private eCharacterGender _gender;

       [SerializeField] private List<AssetReference> _animationList = new List<AssetReference>();

        public override eCharacterGender Gender => _gender;
#if UNITY_EDITOR
        public List<string> GetAnimationNames()
        {
            List<string> names = new List<string>();

            for (int i = 0; i < _animationList.Count; i++)
            {
                names.Add(_animationList[i].editorAsset.name);
            }

            return names;
        }

        public string GetUniqueIdleAnimationName(List<string> exclusionList, int countVariants)
        {
            string idleAnimName = "";

            var allAnimationVariants = GetAnimationNames();
            

            for (int i = 0; i < allAnimationVariants.Count; i++)
            {
                var currentAnimationName = allAnimationVariants[i];
                var containts = exclusionList.Find(x => x == currentAnimationName);
                if (containts == null) return currentAnimationName;
            }

            if (idleAnimName == "")
            {
                var maxCountWithOneAnimation = countVariants / allAnimationVariants.Count;
                int minCount = maxCountWithOneAnimation;
                int index = -1;
                for (int i = 0; i < allAnimationVariants.Count; i++)
                {
                    var animVariants = exclusionList.FindAll(x=>x == allAnimationVariants[i]);
                    if (animVariants != null)
                    {
                        if (animVariants.Count < minCount)
                        {
                            minCount = animVariants.Count;
                            index = i;
                        }
                    }
                }
                idleAnimName = allAnimationVariants[index];
            }

            return idleAnimName;
        }
#endif
    }
}
