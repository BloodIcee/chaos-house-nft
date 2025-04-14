using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ChaosHouse
{
    [CustomEditor(typeof(MatchingHairAndBones))]
    public class MatchingHairAndBonesEditor : Editor
    {
        private MatchingHairAndBones _matching;
        private SerializedProperty _characterPfb;
        private SerializedProperty _gender;
        private SerializedProperty _hairAndBones;

        private ReorderableList reorderableList;
        private List<string> allHairName = new List<string>();

        [SerializeField] private ReorderableList _hairAndBonesList;

        private int _currentlySelectedConversationIndex = -1;

        private readonly Dictionary<string, ReorderableList> _bonesListDict = new Dictionary<string, ReorderableList>();

        private void OnEnable()
        {
            _matching = target as MatchingHairAndBones;


            if (_matching.Character != null)
            {
                InitHairNames();
            }

            _characterPfb = serializedObject.FindProperty("_characterView");
            _gender = serializedObject.FindProperty("_gender");
            _hairAndBones = serializedObject.FindProperty("_hairAndBones");

            if (_matching.Character == null) return;

            _hairAndBonesList = new ReorderableList(serializedObject, _hairAndBones)
            {
                displayAdd = true,
                displayRemove = true,
                draggable = true,                

                drawHeaderCallback = DrawMatchingsHeader,

                drawElementCallback = DrawHairMatchingElement,

                elementHeightCallback = (index) =>
                {
                    return GetHairMathingHeight(_hairAndBones.GetArrayElementAtIndex(index));
                }
            };
        }

        private void InitHairNames()
        {
            var allBonesView = _matching.Character.HairBonesViewController.hairBonesViews;
            for (int i = 0; i < allBonesView.Count; i++)
            {
                allHairName.Add(allBonesView[i].HairName);
            }
           
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_gender);
            EditorGUILayout.PropertyField(_characterPfb);

            if (_matching.Character == null)
            {
                EditorGUILayout.HelpBox("Wait character prefab", MessageType.Info);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            _hairAndBonesList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

        }

        private void DrawMatchingsHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Matching list");
        }

        private void DrawBonesHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Bones");
        }


        private void DrawHairMatchingElement(Rect rect, int index, bool isActive, bool isFocused)
        {

            if (isActive) _currentlySelectedConversationIndex = index;

            var hairMatching = _hairAndBones.GetArrayElementAtIndex(index);

            var name = hairMatching.FindPropertyRelative("_hairName");
            var bones = hairMatching.FindPropertyRelative("_bonesNames");

            string HairMatchingListKey = hairMatching.propertyPath;


            string hairName = name.stringValue;
            int selectedHair = 0;

            if (hairName != null && hairName != "")
            {
                selectedHair = allHairName.FindIndex(x => x == hairName);
            }                        

            selectedHair = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), selectedHair, allHairName.ToArray());
            name.stringValue = allHairName[selectedHair];


            rect.y += EditorGUIUtility.singleLineHeight;

            
            var bonesList = new ReorderableList(hairMatching.serializedObject, bones)
            {
                displayAdd = true,
                displayRemove = true,
                draggable = true,

                drawHeaderCallback = DrawBonesHeader,

                drawElementCallback = (convRect, convIndex, convActive, convFocused) => { DrawBonesElement(hairName, _bonesListDict[HairMatchingListKey], convRect, convIndex, convActive, convFocused); },

                elementHeightCallback = (bonesIndex) =>
                {
                    return GetBonesHeight(_bonesListDict[HairMatchingListKey].serializedProperty.GetArrayElementAtIndex(bonesIndex));
                },

                onAddCallback = (list) =>
                {
                    list.serializedProperty.arraySize++;
                    var addedElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);

                    var bonesNames = _matching.Character.HairBonesViewController.GetHairBonesView(hairName).GetAllBonesName();
                    addedElement.stringValue = bonesNames[0];
                }
            };

            _bonesListDict[HairMatchingListKey] = bonesList;

            bonesList.DoList(rect);      
        }

        private void DrawBonesElement(string hairName, ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
        {

            var bones = list.serializedProperty.GetArrayElementAtIndex(index);

            string bonesName = bones.stringValue;
            int selectectedBones = 0;

            var bonesNames = _matching.Character.HairBonesViewController.GetHairBonesView(hairName).GetAllBonesName();
            if (hairName != null && hairName != "")
            {
                selectectedBones = bonesNames.FindIndex(x => x == bonesName);
            }

            if (selectectedBones < 0) selectectedBones = 0;

            selectectedBones = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width - 20, EditorGUIUtility.singleLineHeight), selectectedBones, bonesNames.ToArray());
            bones.stringValue = bonesNames[selectectedBones];

            if (GUI.Button(new Rect(rect.x + rect.width - 20, rect.y, 20, EditorGUIUtility.singleLineHeight), "X"))
            {                
                list.serializedProperty.DeleteArrayElementAtIndex(index);
                list.DoList(rect);
            }
           
        }


        private float GetHairMathingHeight(SerializedProperty currentMathing)
        {

            var height = EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.singleLineHeight * 5;

            var bonesProp = currentMathing.FindPropertyRelative("_bonesNames");

            for (var d = 0; d < bonesProp.arraySize; d++)
            {
                var bones  = bonesProp.GetArrayElementAtIndex(d);
                height += GetBonesHeight(bones);
            }


            return height;
        }

        private float GetBonesHeight(SerializedProperty bones)
        {            
            return EditorGUIUtility.singleLineHeight;
        }




    }
}
