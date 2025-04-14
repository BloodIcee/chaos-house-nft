using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class PlayerInUI : MonoBehaviour
    {
        [SerializeField] private MaleView _maleView;
        [SerializeField] private FemaleView _femaleView;

        private void Start()
        {
            ShowCharacter();
        }

        private void ShowCharacter()
        {
            CharacterDatabase characterDatabase = Resources.Load<CharacterDatabase>("CharacterDatabase");
            var currentCharacterDataIndex = SaveManager.SavedData.CurrentCharacterDataIndex;
            var currentCharacterData = characterDatabase.CharacterDatas[currentCharacterDataIndex];

            if (currentCharacterData.CharacterGender == eCharacterGender.Female)
            {                
                _femaleView.UpdateView(currentCharacterData);
                _femaleView.gameObject.SetActive(true);
            }
            else
            {                
                _maleView.UpdateView(currentCharacterData);
                _maleView.gameObject.SetActive(true); 
            }
        }
    }

}