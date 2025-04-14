using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "MatchingHairAndBones", menuName = "Configs/MatchingHairAndBones")]
    public class MatchingHairAndBones : IHaveMatching
    {
        [SerializeField] private eCharacterGender _gender;
        [SerializeField] private CharacterView _characterView;

        [SerializeField] private List<HairAndBones> _hairAndBones = new List<HairAndBones>();
        public override eCharacterGender Gender => _gender;
        public CharacterView Character => _characterView;

        public HairAndBones GetHairAndBones(string hairName)
        {
            return _hairAndBones.Find(x=> x.HairName == hairName);
        }
    }

    [System.Serializable]
    public class HairAndBones
    {
        [SerializeField] private string _hairName;
        [SerializeField] private List<string> _bonesNames;

        public string HairName => _hairName;
        public List<string> BonesNames => _bonesNames;
    }
}
