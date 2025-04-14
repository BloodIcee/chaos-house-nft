using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System;

namespace ChaosHouse
{
    [System.Serializable]
    public class CharactedDataField
    {
        [SerializeField] private string _characterDataName;
        public string CharacterDataName => _characterDataName;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CharactedDataField))]
    public class IngredientDrawerUIE : PropertyDrawer
    {
        private int _selectedName = 0;        
        private CharacterDatabase _characterDatabase;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(_characterDatabase ==null) _characterDatabase = Resources.Load<CharacterDatabase>("CharacterDatabase");
            CharacterView characterView = property.serializedObject.targetObject as CharacterView;
            List<string>  _allCharacterName = _characterDatabase.GetCharacterNames(characterView is MaleView?eCharacterGender.Male:eCharacterGender.Female).ToList();

            if (_allCharacterName == null) return;

            _selectedName = _allCharacterName.FindIndex(x=>x == property.FindPropertyRelative("_characterDataName").stringValue);
            if (_selectedName < 0) _selectedName = 0;
            EditorGUI.BeginProperty(position, label, property);
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var nameRect = new Rect(position.x, position.y, position.width, position.height);

            var isChanged = DrawChangableControl(()=> _selectedName =  EditorGUI.Popup(nameRect, _selectedName, _allCharacterName.ToArray()));
            
            property.FindPropertyRelative("_characterDataName").stringValue = _allCharacterName[_selectedName];

            if (isChanged)
            {
                characterView.UpdateView(_characterDatabase.GetCharacterData(_allCharacterName[_selectedName]));
            }
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();

        }

        private bool DrawChangableControl(Action drawer)
        {
            EditorGUI.BeginChangeCheck();
            drawer?.Invoke();
            return EditorGUI.EndChangeCheck();
        }
    }
#endif
    }
