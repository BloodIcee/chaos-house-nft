using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Configs/CharacterDatabase")]
    public class CharacterDatabase : ScriptableObject
    {
        [SerializeField] private List<CharacterData> characterDatas = new List<CharacterData>();

        public IReadOnlyList<CharacterData> CharacterDatas => characterDatas;
        public CharacterData GetLastCharacterData => characterDatas[characterDatas.Count-1];

        private UniqueCharacterOptionsConfig uniqueCharacterOptionsConfig;
        public void Add(CharacterData characterData)
        {
            if (!characterDatas.Contains(characterData)) 
                characterDatas.Add(characterData);
        }

        public void Remove(CharacterData characterData)
        {
            if (characterDatas.Contains(characterData)) characterDatas.Remove(characterData);
        }
        public void Remove(int index)
        {
             characterDatas.RemoveAt(index);
        }

        public void Replace(int index, CharacterData newCharacterData)
        {
            characterDatas[index] = newCharacterData;
        }        
        public int GetCharacterDataIndex(CharacterData characterData) 
        {
            return characterDatas.FindIndex(x=>x==characterData);
        }
        public string[] GetCharacterNames(eCharacterGender gender)
        {
            var characterList = characterDatas.FindAll(x=>x.CharacterGender == gender);
            List<string> namesList = new List<string>();
            characterList.ForEach(x => namesList.Add(x.EntityName));

            return namesList.ToArray();
        }
        public bool ContainsCharacterData(CharacterData characterData, bool editMode = false) 
        {
            InitUniqueCharacterOptionsConfig();
            int indexEditMode = characterDatas.IndexOf(characterData);
            
            for (int i = 0; i < characterDatas.Count; i++)
            {
                if (editMode && indexEditMode > -1 && indexEditMode == i) continue;
                if (characterData.Equals(characterDatas[i], uniqueCharacterOptionsConfig)) return true;
            }

            return false;
        }
        public bool ContainsName (string nameValue)=> characterDatas.Find(x=> x.EntityName == nameValue) != null;
        public CharacterData GetCharacterData (string nameValue) => characterDatas.Find(x=> x.EntityName == nameValue);
        private void InitUniqueCharacterOptionsConfig()
        {
            if (uniqueCharacterOptionsConfig == null) uniqueCharacterOptionsConfig = Resources.Load<UniqueCharacterOptionsConfig>("UniqueCharacterOptionsConfig");
        }

#if UNITY_EDITOR

        public int GetNumberOfVariants(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView)
        {
            int amount = 1;
            InitUniqueCharacterOptionsConfig();

             var optionList = uniqueCharacterOptionsConfig.UniqueOptionsList;

            for (int i = optionList.Count-1; i >= 0 ; i--)
            {
                optionList[i].Init(gender, matchingDatabase, characterView, characterDatas);
                
                if (i == optionList.Count - 1) {
                    amount = optionList[i].GetCountVariants();       
                }
                else
                {
                    amount = (optionList[i].GetCountVariants()) * amount;                    
                }

                if (amount == 0) amount = 1;
            }

            return amount;
        }
        public CharacterData GetUniqueCharacterData(eCharacterGender gender, MatchingDatabase matchingDatabase, CharacterView characterView)
        {
            InitUniqueCharacterOptionsConfig();
            var optionConfig = uniqueCharacterOptionsConfig;
            var countVariants = GetNumberOfVariants(gender, matchingDatabase, characterView);

            //Animations 
            IdleAnimationOption idleAnimationOption = new IdleAnimationOption();
            idleAnimationOption.Init(gender, matchingDatabase, characterView, CharacterDatas as List<CharacterData>);
            string animationName = idleAnimationOption.GetIdleAnimationName(optionConfig.Contains(idleAnimationOption), countVariants);

            /// get clothes
            ClothesOption clothesOption = new ClothesOption();
            clothesOption.Init(gender, matchingDatabase, characterView, characterDatas);
            Clothes clothes = clothesOption.GetClothes(optionConfig.Contains(clothesOption), countVariants);

            /// get face 
            FaceOption faceOption = new FaceOption();
            faceOption.Init(gender, matchingDatabase, characterView, characterDatas);
            Face face = faceOption.GetFace();

            // get body skin
            BodyOption bodyOption = new BodyOption();
            bodyOption.Init(gender, matchingDatabase, characterView, characterDatas);
            Body body = bodyOption.GetBody(optionConfig.Contains(bodyOption), countVariants);

            // get hair 
            HairOption hairOption = new HairOption();
            hairOption.Init(gender, matchingDatabase, characterView, characterDatas);
            Hair hair = hairOption.GetHair(optionConfig.Contains(hairOption), countVariants);

            /// get eyelashas
            EyelashesOption eyelashesOption = new EyelashesOption();
            eyelashesOption.Init(gender, matchingDatabase, characterView, characterDatas);
            Eyelashes eyelashes = eyelashesOption.GetEyelashes(optionConfig.Contains(eyelashesOption), countVariants);

            // get accessories
            AccessoryOption accessoryOption = new AccessoryOption();
            accessoryOption.Init(gender, matchingDatabase, characterView, characterDatas);
            Accessory accessory = accessoryOption.GetAccessory(optionConfig.Contains(accessoryOption), countVariants);


            // get facial hair
            FacialHairOption facialHairOption = new FacialHairOption();
            facialHairOption.Init(gender, matchingDatabase, characterView, characterDatas);
            FacialHair facialHair = facialHairOption.GetFacialHair(optionConfig.Contains(facialHairOption), countVariants);

            int countFemale = characterDatas.FindAll(x=>x.CharacterGender == eCharacterGender.Female).Count;
            int countMale = characterDatas.FindAll(x => x.CharacterGender == eCharacterGender.Male).Count;

            string characterName = gender == eCharacterGender.Female? $"Female{countFemale+1}" : $"Male{countMale + 1}";
            CharacterData characterData = new CharacterData(characterName, gender, clothes, face, body, hair, eyelashes, accessory, facialHair);

            characterData.SetIdleAnimation(animationName);

            return characterData;
        }
#endif
    }
}
