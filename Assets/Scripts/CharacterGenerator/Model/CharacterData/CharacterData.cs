using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [System.Serializable]
    public class CharacterData: IUniqueEntity
    {
        [SerializeField] private string _name;
        [SerializeField] private eCharacterGender _type;
        [SerializeField] private string _idleAnimation;
        [SerializeField] private Accessory _accessories;
        [SerializeField] private Clothes _clothes;
        [SerializeField] private Face _face;
        [SerializeField] private Body _body;
        [SerializeField] private Eyelashes _eyelashes;
        [SerializeField] private Hair _hair;
        [SerializeField] private FacialHair _facialHair;
        public Accessory Accessory => _accessories;
        public Clothes Clothes => _clothes;
        public Face FaceData => _face;
        public Body BodyData => _body;
        public Eyelashes EyelashesData => _eyelashes;
        public Hair HairData => _hair;
        public eCharacterGender CharacterGender => _type;
        public string EntityName => _name;
        public string IdleAnimationName => _idleAnimation;
        public FacialHair FacialHairData =>_facialHair;
        public string GetID()
        {
            return JsonUtility.ToJson(this);
        }

        public CharacterData(string nameValue, eCharacterGender characteTypeValue, Clothes clothes, Face faceValue, Body bodyValue, Hair hair, Eyelashes eyelashes = null, Accessory accessory = null, FacialHair facialHair = null)
        {
            this._name = nameValue;
            this._clothes = clothes;
            this._type = characteTypeValue;
            this._accessories = accessory;
            this._face = faceValue;
            this._body = bodyValue;
            this._eyelashes = eyelashes;
            this._hair = hair;
            this._facialHair = facialHair;
        }

        public bool Equals(object obj, UniqueCharacterOptionsConfig optionConfig)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CharacterData characterData = (CharacterData)obj;
                if (this._type != characterData.CharacterGender) return false;
                if(optionConfig.Contains(new ClothesOption()) && JsonUtility.ToJson(this._clothes) != JsonUtility.ToJson(characterData.Clothes)) return false;
                if(optionConfig.Contains(new FaceOption()) && JsonUtility.ToJson(this._face) != JsonUtility.ToJson(characterData.FaceData)) return false;
                if(optionConfig.Contains(new BodyOption()) && JsonUtility.ToJson(this._body) != JsonUtility.ToJson(characterData.BodyData)) return false;
                if(optionConfig.Contains(new HairOption()) && JsonUtility.ToJson(this._hair) != JsonUtility.ToJson(characterData.HairData)) return false;
                if(optionConfig.Contains(new EyelashesOption()) && JsonUtility.ToJson(this._eyelashes) != JsonUtility.ToJson(characterData.EyelashesData)) return false;
                if(optionConfig.Contains(new AccessoryOption()) && JsonUtility.ToJson(this._accessories) != JsonUtility.ToJson(characterData.Accessory)) return false;
                if(optionConfig.Contains(new IdleAnimationOption()) && this.IdleAnimationName != characterData.IdleAnimationName) return false;
                               
                return true;
            }

        }

        public override bool Equals(object obj)
        {
            CharacterData characterData = (CharacterData)obj;
            var thisData = JsonUtility.ToJson(this);
            var objData = JsonUtility.ToJson(characterData);
            return thisData == objData;
        }
        public void SetIdleAnimation(string animName)
        {
            _idleAnimation = animName;
        }

    }
}
