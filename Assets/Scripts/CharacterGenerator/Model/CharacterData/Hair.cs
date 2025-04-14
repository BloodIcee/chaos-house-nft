using System;
using UnityEngine;

namespace ChaosHouse
{
    [Serializable]
    public class Hair : CharacterPart
    {
        [SerializeField] private string _textureName;

        public string TextureName => _textureName;
        public Hair()
        {
        }

        public Hair(string nameValue, string[] materialNames, string textureNameValue) : base(nameValue, materialNames)
        {
            _textureName = textureNameValue;
        }

        public void SetTextureName(string textureNameValue)
        {
            _textureName = textureNameValue;
        }

        public bool TextureIsMatch(string textureName)
        {
            return _textureName == textureName;
        }
    }
}