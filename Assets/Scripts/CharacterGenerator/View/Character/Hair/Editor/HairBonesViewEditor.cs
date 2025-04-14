using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;

namespace ChaosHouse
{
    [CustomEditor(typeof(HairBonesView)), CanEditMultipleObjects]
    public class HairBonesViewEditor : Editor
    {
        private SerializedProperty _hairNameProp;
        private SerializedProperty _characterViewProp;

        private HairBonesView _hairBonesView;
        private CharacterView _characterView;

        private List<string> allNames;
        private int selectedBones = 0;
        private void OnEnable()
        {
            _hairBonesView = target as HairBonesView;

            _hairNameProp = serializedObject.FindProperty("_hairName");
            _characterViewProp = serializedObject.FindProperty("_characterView");

           
            _characterView = _hairBonesView.CharacterViewValue as CharacterView;
            
            if (_characterView == null) return;

            allNames = _characterView.GetAllHairNames();

            selectedBones = allNames.FindIndex(x=>x== _hairNameProp.stringValue); 
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_characterViewProp);            

            if (_hairBonesView.CharacterViewValue == null) {
                EditorGUILayout.LabelField("wait character view");
                serializedObject.ApplyModifiedProperties();
                return;
            }

           if(allNames ==null) allNames = _characterView.GetAllHairNames();
            selectedBones = EditorGUILayout.Popup("Bones name", selectedBones, allNames.ToArray());
            _hairNameProp.stringValue = allNames[selectedBones];

            EditorUtility.SetDirty(_hairBonesView.gameObject);

            serializedObject.ApplyModifiedProperties();

        }
    }
}
