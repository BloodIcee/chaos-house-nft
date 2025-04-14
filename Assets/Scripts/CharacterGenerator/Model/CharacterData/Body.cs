using System;
using UnityEngine;

namespace ChaosHouse
{
    [Serializable]
    public class Body : CharacterPart
    {
        [SerializeField] private string _textureName;

        public string TextureName => _textureName;
        public Body()
        {

        }

        public Body(string nameValue, string[] materialNames, string textureNameValue) : base(nameValue, materialNames)
        {
            _textureName = textureNameValue;
        }

        public bool TextureNameIsMatch(string textureName)
        {
            return _textureName == textureName;
        }
        public void SetTextureName(string textureNameValue)
        {
            _textureName = textureNameValue;
        }
    }
}
