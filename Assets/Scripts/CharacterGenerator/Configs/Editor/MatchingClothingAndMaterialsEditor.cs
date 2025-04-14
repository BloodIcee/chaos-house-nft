using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace ChaosHouse
{
    [CustomEditor(typeof(MatchingClothingAndMaterials))]
    public class MatchingClothingAndMaterialsEditor : Editor
    {
        private MatchingClothingAndMaterials _matching;
        private SerializedProperty _characterPfb;
        private SerializedProperty _gender;
        private SerializedProperty _clothMaterialMatching;

        private ReorderableList reorderableList;
        private List<string> allClothesName = new List<string>();

        private void OnEnable()
        {
            _matching = target as MatchingClothingAndMaterials;

            

            _characterPfb = serializedObject.FindProperty("_characterPfb");
            _gender = serializedObject.FindProperty("_gender");            
            _clothMaterialMatching = serializedObject.FindProperty("_clothMaterialMatching");           

            reorderableList = new ReorderableList(serializedObject, _clothMaterialMatching, true, true, true, true);
           
            reorderableList.drawElementCallback += drawElementCallback;
            reorderableList.elementHeightCallback += elementHeightCallback;
            
        }

        private float elementHeightCallback(int index)
        {
           
            SerializedProperty element = _clothMaterialMatching.GetArrayElementAtIndex(index);
            float height = EditorGUIUtility.standardVerticalSpacing;

            var clothesNameProp = element.FindPropertyRelative("_clothName");
            height += EditorGUI.GetPropertyHeight(clothesNameProp, true) + EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty _materials = element.FindPropertyRelative("_materials");
            height += EditorGUI.GetPropertyHeight(_materials, true) + EditorGUIUtility.standardVerticalSpacing;

            return height + EditorGUIUtility.standardVerticalSpacing;
        }

        private void drawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            allClothesName = _matching.CharacterPfb.GetAllClothesNames();

            var element = _clothMaterialMatching.GetArrayElementAtIndex(index);           

            var clothesNameProp = element.FindPropertyRelative("_clothName");                                  
            string clothesName = clothesNameProp.stringValue;
            int selectedClothes = 0;

            if (clothesName != null && clothesName!= "")
            {
                selectedClothes = allClothesName.FindIndex(x => x == clothesName);
            }

  

            selectedClothes = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width*0.25f, EditorGUIUtility.singleLineHeight), selectedClothes, allClothesName.ToArray());            
            clothesNameProp.stringValue = allClothesName[selectedClothes];

            var materialsArrayProp = element.FindPropertyRelative("_materials");
            EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.3f, rect.y, rect.width - rect.width * 0.3f, EditorGUI.GetPropertyHeight(materialsArrayProp, true)), materialsArrayProp, true);
       
        }

 

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_gender);
            EditorGUILayout.PropertyField(_characterPfb);

            if (_characterPfb.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Wait characterPfb ", MessageType.Info);
                serializedObject.ApplyModifiedProperties();
                return;
            }
            reorderableList.DoLayoutList();

            
            serializedObject.ApplyModifiedProperties();

        }
    }
}
