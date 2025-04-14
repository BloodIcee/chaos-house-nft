using ChaosHouse;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CharacterGenerator
{
    public class EditMode : CreateMode
    {
        private int _characterDataIndex = -1;
        private CharacterData _charactedData_temp;
        public EditMode(eCharacterGender gender, CharacterDatabase characterDatabaseValue, CharacterView femalePrefabValue, CharacterView malePrefabValue, MatchingDatabase matchingDatabaseValue, CharacterData characterDataValue) 
            : base(gender, characterDatabaseValue, femalePrefabValue, malePrefabValue, matchingDatabaseValue, characterDataValue)
        {
            currentCharacterData = characterDataValue;
            _characterDataIndex = characterDatabase.GetCharacterDataIndex(currentCharacterData);
            selectedGender = gender;

            this.InitCharacterData();
            UpdateCharactersNames();
            UpdatePrefab();
            UpdatePreview();
        }
        

        protected override void InitCharacterData()
        {

            var json = JsonUtility.ToJson(currentCharacterData);
            _charactedData_temp = JsonUtility.FromJson<CharacterData>(json);

            selectedGender = _charactedData_temp.CharacterGender;

            this.idleAnimationName = _charactedData_temp.IdleAnimationName;
            this.selectedGender = _charactedData_temp.CharacterGender;
            this.characterName = _charactedData_temp.EntityName;
            this.eyelashes = _charactedData_temp.EyelashesData;
            this.accessory = _charactedData_temp.Accessory;
            this.clothes = _charactedData_temp.Clothes;
            this.hair = _charactedData_temp.HairData;
            this.face = _charactedData_temp.FaceData;
            this.body = _charactedData_temp.BodyData;
            this.facialHair = _charactedData_temp.FacialHairData;

            loadedClothesMaterials = LoadAsyncMaterials(clothes.MaterialNames);
            loadedFaceMaterials = LoadAsyncMaterials(face.MaterialNames);
            MaterialLoader materialLoader = new MaterialLoader(face.MaterialNames);
            materialLoader.onLoaded += () =>
              {
                  loadedFaceMaterials = materialLoader.materialList;
                  loadedFaceMaterials = SortMaterials(face.MaterialNames, loadedFaceMaterials);
              };           

            loadedAccessoryMaterials = LoadAsyncMaterials(accessory.MaterialNames);
            loadedBodyMaterials = LoadAsyncMaterials(body.MaterialNames);
            loadedEyelashesMaterials = LoadAsyncMaterials(eyelashes.MaterialNames);
            loadedHairMaterials = LoadAsyncMaterials(hair.MaterialNames);

            currentCharacterPrefab.UpdateView(_charactedData_temp);
            UpdatePreview();
        }

        protected override void Save()
        {
 
            currentCharacterData = new CharacterData(
                                  this.characterName,
                                  this.selectedGender,
                                  this.clothes,
                                  this.face,
                                  this.body,
                                  this.hair,
                                  this.eyelashes,
                                  this.accessory,
                                  this.facialHair
                                  );

            currentCharacterData.SetIdleAnimation(this.idleAnimationName);

            if (characterDatabase.ContainsCharacterData(currentCharacterData, true))
            {
                EditorUtility.DisplayDialog("Error", "A character with such data is already in the database!", "OK");
                return;
            }

            characterDatabase.Replace(_characterDataIndex, currentCharacterData);

            UpdateCharactersNames();

            if (characterDatabase.CharacterDatas.Count > 0)
            {
                selectedPressetName = characterDatabase.CharacterDatas.Count - 1;
                currentCharacterData = characterDatabase.GetLastCharacterData;
            }

            ChangeWindow(new PreviewMode(characterDatabase, femalePrefab, malePrefab, matchingDatabase));
        }

        protected override void Cancel()
        {
            _charactedData_temp = null;
            currentCharacterData = null;

            if (characterDatabase.CharacterDatas.Count > 0)
            {
                var characterNames = characterDatabase.GetCharacterNames(selectedGender);
                selectedPressetName = characterNames.Length - 1;
                currentCharacterData = characterDatabase.GetCharacterData(characterNames[selectedPressetName]);

            }

            SaveGender(selectedGender);
            UpdateCharacterView();

            ChangeWindow(new PreviewMode(characterDatabase, femalePrefab, malePrefab, matchingDatabase));

        }

    }
}
