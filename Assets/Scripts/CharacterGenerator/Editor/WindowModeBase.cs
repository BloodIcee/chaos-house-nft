using ChaosHouse;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CharacterGenerator
{
    public abstract class WindowModeBase
    {
        protected EditorCoroutine currentCoroutine;
        public abstract void Draw();

        protected string[] pressetNames;
        protected CharacterView femalePrefab;
        protected CharacterView malePrefab;
        protected int selectedPressetName = 0;
        protected eCharacterGender selectedGender;
        protected CharacterDatabase characterDatabase;     
        protected MatchingDatabase matchingDatabase;

        public event Action<WindowModeBase> windowModeChanged;
        public Editor preview { get; protected set; }
        public CharacterView currentCharacterPrefab { get; protected set; }
        public CharacterData currentCharacterData { get; protected set; }
        private CharacterData _previousCharacterData;
        public GameObject CurrentCharacterGO => currentCharacterPrefab == null ? null: currentCharacterPrefab.gameObject;
        public WindowModeBase(CharacterDatabase characterDatabaseValue, CharacterView femalePrefabValue, CharacterView malePrefabValue, MatchingDatabase matchingDatabaseValue, CharacterData characterData = null)
        {
            currentCharacterData = characterData;

            characterDatabase = characterDatabaseValue;
            matchingDatabase = matchingDatabaseValue;

            femalePrefab = femalePrefabValue;
            malePrefab = malePrefabValue;           
           
            UpdateCharactersNames();
            UpdateCharacterView();

            
        }



        protected void DrawTooglableControl(Action drawer, bool condition)
        {
            GUI.enabled = condition;
            drawer?.Invoke();
            GUI.enabled = true;

        }

        protected bool DrawChangableControl(Action drawer)
        {
            EditorGUI.BeginChangeCheck();
            drawer?.Invoke();
            return EditorGUI.EndChangeCheck();
        }

        protected void UpdateCharactersNames()
        {
            pressetNames = characterDatabase.GetCharacterNames(selectedGender);
            if (pressetNames == null || pressetNames.Length ==0) pressetNames = new string[1] { "none" };
        }
        protected void UpdatePrefab()
        {
            currentCharacterPrefab = selectedGender == eCharacterGender.Female ? femalePrefab : malePrefab;
        }

        protected void DrawPreview(bool fullScreen)
        {

            Rect rect = GUILayoutUtility.GetRect(Screen.width, !fullScreen ? 400 : Screen.height - EditorGUIUtility.singleLineHeight * 6);

            if (preview != null)
            {
                preview.OnInteractivePreviewGUI(rect, EditorStyles.whiteLabel);            
            }            
        }

        protected void DrawFooter()
        {
            if (currentCharacterData == null) return;

            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox(currentCharacterData.GetID(), MessageType.Info);
            EditorGUILayout.HelpBox($"Max: {characterDatabase.GetNumberOfVariants(selectedGender, matchingDatabase, currentCharacterPrefab)}", MessageType.None);
            GUILayout.EndHorizontal();
        }

        public void UpdatePreview()
        {
            if (preview != null) Editor.DestroyImmediate(preview);

            preview = (currentCharacterPrefab == null) ? null : Editor.CreateEditor(currentCharacterPrefab.gameObject);            
        }

        protected List<Material> LoadAsyncMaterials(string[] materials)
        {
            if (materials == null) return new List<Material>();

            List<Material> materialList = new List<Material>();

            for (int i = 0; i < materials.Length; i++)
            {
                AddressablesLoader.LoadAsset<Material>(materials[i], (AsyncOperationHandle<Material> obj) => { materialList.Add(obj.Result); });
            }

            return materialList;

        }
 
        protected List<Material> SortMaterials(string[] materialsOrder, List<Material> listForSorted)
        {
            List<Material> sortedList = new List<Material>();

            for (int i = 0; i < materialsOrder.Length; i++)
            {
                sortedList.Add(listForSorted.Find(x => x.name == materialsOrder[i]));
            }

            return sortedList;
        }


        protected string[] GetMaterialNames(List<Material> materials)
        {
            string[] materialNames = new string[materials.Count];

            for (int i = 0; i < materialNames.Length; i++) materialNames[i] = materials[i].name;

            return materialNames;
        }

        protected void OnlyDrawMaterialsField(List<Material> materials)
        {
            GUI.enabled = false;
            for (int i = 0; i < materials.Count; i++)
            {
                EditorGUILayout.ObjectField(materials[i], typeof(Material));
            }
            GUI.enabled = true;
        }

        protected void DrawMaterialsField(List<Material> materials)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i] = (Material)EditorGUILayout.ObjectField(materials[i], typeof(Material));
            }
        }

        protected async void AsyncAction(Action action, int millisecondsDelay)
        {
            await Task.Delay(millisecondsDelay);
            action?.Invoke();
        }
        protected async void AsyncAction(Action action, Task task)
        {
            await task;
            action?.Invoke();
        }
        public void UpdateCharacterView()
        {
            if (_previousCharacterData == null) _previousCharacterData = currentCharacterData;
            else
            {
                if (_previousCharacterData.Equals(currentCharacterData)) return;
                else _previousCharacterData = currentCharacterData;                           
            }

            UpdatePrefab();

            if (pressetNames[selectedPressetName] == "none") return;
            currentCharacterData = characterDatabase.GetCharacterData(pressetNames[selectedPressetName]);
            currentCharacterPrefab.UpdateView(currentCharacterData);
        }

        protected virtual void ChangeWindow(WindowModeBase windowModeBase)
        {

            windowModeChanged?.Invoke(windowModeBase);
        }

        public void StopAnimation()
        {
            if(currentCoroutine!=null) EditorCoroutineUtility.StopCoroutine(currentCoroutine);
        }
        public IEnumerator PlayAnimation(AnimationClip clip)
        {            
            float time = 0.0f;
            float tempTime = 0f;

            float startTime = Time.realtimeSinceStartup;
            float stopTime = startTime + clip.length;
            
            while (true)
            {
                AnimationMode.StartAnimationMode();
                AnimationMode.BeginSampling();
                tempTime = Time.realtimeSinceStartup;
                Debug.LogFormat("Time since startup: {0} s", Time.realtimeSinceStartup);
                if(Time.realtimeSinceStartup < stopTime) AnimationMode.SampleAnimationClip(CurrentCharacterGO, clip, time);
                time += Time.realtimeSinceStartup - tempTime;
                AnimationMode.EndSampling();

                preview.Repaint();

                yield return null;
            }                    
        }

        protected void SaveGender(eCharacterGender eCharacterGender) => PlayerPrefs.SetInt("Gender", eCharacterGender == eCharacterGender.Female ? 1 : 0);
        protected eCharacterGender GetSavedGender() => PlayerPrefs.GetInt("Gender") == 1? eCharacterGender.Female : eCharacterGender.Male;
    }
}
