using ChaosHouse;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using ChaosHouse.CharacterGenerator;

namespace CharacterGenerator
{
    public class CreateMode : WindowModeBase
    {
        protected Vector2 scroll;        
        protected float time;

        protected List<Material> loadedFaceMaterials = new List<Material>();
        protected List<Material> loadedBodyMaterials = new List<Material>();
        protected List<Material> loadedHairMaterials = new List<Material>();
        protected List<Material> loadedClothesMaterials = new List<Material>();
        protected List<Material> loadedEyelashesMaterials = new List<Material>();
        protected List<Material> loadedAccessoryMaterials = new List<Material>();
        protected List<Material> loadedFacialHairMaterials = new List<Material>();

        protected string characterName;
        protected string idleAnimationName;
        protected Eyelashes eyelashes;
        protected Accessory accessory;
        protected Clothes clothes;
        protected Hair hair;
        protected Face face;
        protected Body body;
        protected FacialHair facialHair;

        protected Toolbar toolbar = new Toolbar();

        public CreateMode(eCharacterGender gender, CharacterDatabase characterDatabaseValue, CharacterView femalePrefabValue, CharacterView malePrefabValue, MatchingDatabase matchingDatabaseValue,  CharacterData characterData) 
            : base(characterDatabaseValue, femalePrefabValue, malePrefabValue, matchingDatabaseValue,  characterData)
        {
            currentCharacterData = characterData;
            selectedGender = gender;
            UpdateCharactersNames();
            UpdatePrefab();
            this.InitCharacterData();

            toolbar.SetActionOnChanged(UpdatePreview);
        }
         
        protected virtual void InitCharacterData()
        {            
            currentCharacterData = characterDatabase.GetUniqueCharacterData(selectedGender, matchingDatabase, currentCharacterPrefab);

            this.selectedGender = currentCharacterData.CharacterGender;
            this.characterName = currentCharacterData.EntityName;
            this.eyelashes = currentCharacterData.EyelashesData;
            this.accessory = currentCharacterData.Accessory;
            this.clothes = currentCharacterData.Clothes;
            this.hair = currentCharacterData.HairData;
            this.face = currentCharacterData.FaceData;
            this.body = currentCharacterData.BodyData;
            this.idleAnimationName = currentCharacterData.IdleAnimationName;
            this.facialHair = currentCharacterData.FacialHairData;

            loadedClothesMaterials = LoadAsyncMaterials(clothes.MaterialNames);

            MaterialLoader materialLoader = new MaterialLoader(face.MaterialNames);
            materialLoader.onLoaded += () =>
            {
                loadedFaceMaterials = materialLoader.materialList;
                loadedFaceMaterials = SortMaterials(face.MaterialNames, loadedFaceMaterials);
            };

            loadedAccessoryMaterials = LoadAsyncMaterials(accessory.MaterialNames);
            loadedBodyMaterials = LoadAsyncMaterials(body.MaterialNames);
           if(currentCharacterData.CharacterGender == eCharacterGender.Female) loadedEyelashesMaterials = LoadAsyncMaterials(eyelashes.MaterialNames);
            loadedHairMaterials = LoadAsyncMaterials(hair.MaterialNames);
            loadedFacialHairMaterials = loadedHairMaterials;

            currentCharacterPrefab.UpdateView(currentCharacterData);
            UpdatePreview();
        }

        public override void Draw()
        {           
            EditorGUILayout.BeginHorizontal();
            {                
                characterName = EditorGUILayout.TextField(characterName);

                if (GUILayout.Button("Save")) Save();

                if (GUILayout.Button("Cancel")) Cancel();

            }

            EditorGUILayout.EndHorizontal();

            /// animation 
            var matchingAnimations= matchingDatabase.GetMatchingConfig<IdleAnimationConfig>();
            if (matchingAnimations != null)
            {
                List<string> allAnimationNames = matchingAnimations.GetAnimationNames();
                int selectedAnimationIndex = allAnimationNames.FindIndex(x=>x == idleAnimationName);
                if (selectedAnimationIndex < 0) selectedAnimationIndex = 0;

                bool animationChanged = DrawChangableControl(() => selectedAnimationIndex = EditorGUILayout.Popup(selectedAnimationIndex, allAnimationNames.ToArray()));

                AnimationClip animationClip = currentCharacterPrefab.GetAnimationClips()[selectedAnimationIndex];
                float startTime = 0.0f;
                float stopTime = animationClip.length;

                bool timeChanged = DrawChangableControl(() =>  time = EditorGUILayout.Slider(time, startTime, stopTime));
                
                if (animationChanged || timeChanged)
                {
                   idleAnimationName = allAnimationNames[selectedAnimationIndex];

                    AnimationMode.StartAnimationMode();
                    AnimationMode.BeginSampling();
                    AnimationMode.SampleAnimationClip(CurrentCharacterGO, animationClip, time);
                    AnimationMode.EndSampling();
                    UpdatePreview();
                    
                }
                                
            }

            ////////// character settings
            float height = Screen.height - EditorGUIUtility.singleLineHeight - 2 - (Screen.width < 400 ? Screen.width : 400);

            scroll = EditorGUILayout.BeginScrollView(scroll, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Height(height));
            {
                EditorGUILayout.BeginVertical(GUILayout.Height(height - 4));
                {

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Character Settings", EditorStyles.boldLabel);                    

                    toolbar.Add<Face>(new FaceToolbar(face, loadedFaceMaterials));
                    toolbar.Add<Body>(new BodyToolbar(body, loadedBodyMaterials, matchingDatabase, currentCharacterPrefab));
                    toolbar.Add<Clothes>(new ClothesToolbar(clothes, loadedClothesMaterials, matchingDatabase, currentCharacterPrefab));
                    toolbar.Add<Accessory>(new AccessoryToolbar(accessory, loadedAccessoryMaterials, matchingDatabase, currentCharacterPrefab));
                    toolbar.Add<Hair>(new HairToolbar(hair, loadedHairMaterials, matchingDatabase, currentCharacterPrefab));

                    if(selectedGender == eCharacterGender.Female) toolbar.Add<Eyelashes>(new EyelashesToolbar(eyelashes, loadedEyelashesMaterials, matchingDatabase, currentCharacterPrefab));
                    else toolbar.Add<FacialHair>(new FacialToolbar(this.facialHair, loadedFacialHairMaterials, matchingDatabase, currentCharacterPrefab));

                    toolbar.Draw();
                                         
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();


            DrawPreview(false);
        }

        protected void UpdateCurrentCharacterData()
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

            currentCharacterData.SetIdleAnimation(idleAnimationName);
        }
        protected virtual void Save()
        {
            if (characterDatabase.ContainsName(characterName))
            {
                EditorUtility.DisplayDialog("Error", "Change character name", "OK");
                return;
            }

            UpdateCurrentCharacterData();

            if (characterDatabase.ContainsCharacterData(currentCharacterData))
            {
                EditorUtility.DisplayDialog("Error", "A character with such data is already in the database!", "OK");
                return;
            }
            
            characterDatabase.Add(currentCharacterData);

            UpdateCharactersNames();

            if (characterDatabase.CharacterDatas.Count > 0)
            {
                selectedPressetName = characterDatabase.CharacterDatas.Count - 1;
                currentCharacterData = characterDatabase.GetLastCharacterData;
            }

            ChangeWindow(new PreviewMode(characterDatabase, femalePrefab, malePrefab, matchingDatabase));
        }

        protected virtual void Cancel()
        {
            currentCharacterData = null;

            if (pressetNames.Length > 1)
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
