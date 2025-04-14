using UnityEngine;
using UnityEditor;
using ChaosHouse;
using Unity.EditorCoroutines.Editor;

namespace CharacterGenerator
{
    public class CharacterGeneratorWindow : EditorWindow
    {
        #region Call Window
        [MenuItem("Pawsome/Character Manager")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<CharacterGeneratorWindow>(false, "Character Manager", true);
            window.minSize = new Vector2(600, 600);
        }
        #endregion

        private CharacterView femalePrefab;
        private CharacterView malePrefab;

        private CharacterDatabase characterDatabase;

        private WindowModeBase currentWondowMode;
        private MatchingDatabase matchingDatabase;
        
        // animation
        private float sampleTime = 0.0f;
        private AnimationClip animationClip;
        private void OnEnable()
        {
            AddressablesLoader.LoadEnded += AddressablesLoader_LoadEnded;

            characterDatabase = Resources.Load<CharacterDatabase>("CharacterDatabase");
            femalePrefab = Resources.Load<CharacterView>("characterFemaleGenerated");
            malePrefab = Resources.Load<CharacterView>("characterMaleGenerated");
            matchingDatabase = Resources.Load<MatchingDatabase>("MatchingDatabase");

            if (Application.isPlaying) return;

            currentWondowMode = new PreviewMode(characterDatabase, femalePrefab, malePrefab, matchingDatabase);

            currentWondowMode.windowModeChanged += CurrentWondowMode_windowModeChanged;

           
        }
        private void AddressablesLoader_LoadEnded()
        {

            //currentWondowMode.UpdateCharacterView();
            currentWondowMode.UpdatePreview();
        }

        private void Update()
        {           
            //if (animationClip == null)
            //{
            //    animationClip = currentWondowMode.currentCharacterPrefab.GetCurentAnimationClip(currentWondowMode.currentCharacterData.IdleAnimationName);
            //}

            //sampleTime += 0.01f;
            //if (sampleTime > animationClip.length) sampleTime = 0f;

            //Debug.Log(Time.deltaTime);

            //AnimationMode.StartAnimationMode();
            //AnimationMode.BeginSampling();
            //AnimationMode.SampleAnimationClip(currentWondowMode.CurrentCharacterGO, animationClip, sampleTime);
            //AnimationMode.StopAnimationMode();

            //currentWondowMode.UpdatePreview();
        }

        private void CurrentWondowMode_windowModeChanged(WindowModeBase obj)
        {
            currentWondowMode.windowModeChanged -= CurrentWondowMode_windowModeChanged;

            currentWondowMode = obj;

            currentWondowMode.windowModeChanged += CurrentWondowMode_windowModeChanged;


        }

        

        private void OnDisable()
        {
            //currentWondowMode.StopAnimation();
            currentWondowMode.windowModeChanged -= CurrentWondowMode_windowModeChanged;       
        }

        
        private void OnGUI()
        {
            if (Application.isPlaying) return;

            if (currentWondowMode != null)
            {                
                currentWondowMode.Draw();
               
            }



            EditorUtility.SetDirty(characterDatabase);



        }



    }
}
