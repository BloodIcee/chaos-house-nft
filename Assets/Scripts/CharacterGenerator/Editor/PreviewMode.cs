using ChaosHouse;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CharacterGenerator
{
    public class PreviewMode : WindowModeBase
    {
        public PreviewMode(CharacterDatabase characterDatabaseValue, CharacterView femalePrefabValue, CharacterView malePrefabValue, MatchingDatabase matchingDatabase) 
            : base(characterDatabaseValue, femalePrefabValue, malePrefabValue, matchingDatabase)
        {
            selectedGender = GetSavedGender();
            UpdatePrefab();
            UpdateCharactersNames();

            currentCharacterData = characterDatabase.GetCharacterData(pressetNames[selectedPressetName]);
            
            UpdateCharacterView();
            UpdatePreview();            

        }

        public override void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            {

                DrawTooglableControl(() =>
                {
                    bool genderUpdated = DrawChangableControl(() => selectedGender = (eCharacterGender)EditorGUILayout.EnumPopup(selectedGender));
                    if (genderUpdated)
                    {
                        this.UpdateCharactersNames();
                        this.selectedPressetName = 0;
                        currentCharacterData = characterDatabase.GetCharacterData(pressetNames[selectedPressetName]);                        
                        UpdateCharacterView();
                        UpdatePreview();
                        
                    }

                     bool pressetUpdated = DrawChangableControl(() => selectedPressetName = EditorGUILayout.Popup(selectedPressetName, pressetNames));

                    if (pressetUpdated)
                    {
                        currentCharacterData = characterDatabase.GetCharacterData(pressetNames[selectedPressetName]);
                        currentCharacterData = characterDatabase.GetCharacterData(pressetNames[selectedPressetName]);
                        UpdateCharacterView();
                        UpdatePreview();
                    }

                    if (pressetNames[selectedPressetName] != "none")
                    {
                        if (GUILayout.Button("Edit"))
                        {
                            ChangeWindow(new EditMode(selectedGender, characterDatabase, femalePrefab, malePrefab, matchingDatabase, currentCharacterData));
                        }
                    }


                    if (GUILayout.Button("Remove"))
                    {
                        currentCharacterData = characterDatabase.GetCharacterData(pressetNames[selectedPressetName]);
                        characterDatabase.Remove(currentCharacterData);
                        UpdateCharactersNames();

                        if (pressetNames.Length > 0)
                        {
                            this.selectedPressetName = pressetNames.Length - 1;                            
                        }

                        UpdateCharacterView();
                    }

                }, characterDatabase.CharacterDatas.Count > 0);

                if (GUILayout.Button("New"))
                {
                    currentCharacterData = null;
                    ChangeWindow(new CreateMode(selectedGender, characterDatabase, femalePrefab, malePrefab, matchingDatabase, null));
                    UpdatePreview();
                }

            }
            EditorGUILayout.EndHorizontal();

            DrawPreview(true);
            DrawFooter();       
        }

      
    }
}
