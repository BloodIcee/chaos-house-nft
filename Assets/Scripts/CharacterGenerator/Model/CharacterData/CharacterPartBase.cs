using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ChaosHouse
{
    [Serializable]
    public class CharacterPart
    {
        [SerializeField] private string _partName;
        [SerializeField] private string[] _materialNames;

        public string PartName => _partName;
        public string [] MaterialNames => _materialNames;

        public CharacterPart(string nameValue, string[] materialNames)
        {
            _partName = nameValue;
            _materialNames = materialNames;            

        }
        public CharacterPart()
        {

        }

        public bool NameMaterialsIsMatch(List<string> nameList)
        {
            for (int i = 0; i < _materialNames.Length; i++)
            {
                if (!nameList.Contains(_materialNames[i])) return false;
            }
            return true;
        }
        public void SetName(string nameValue) => _partName = nameValue;
        public void SetMaterialNames(string [] materialNamesValue) => _materialNames = materialNamesValue;
    }
}
