using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ChaosHouse
{
    public class RoyaleModeSettings : LevelMode
    {
        [SerializeField] private Bed _activeBed;
        [SerializeField] private PlayerSleepRoyale _maleView;
        [SerializeField] private PlayerSleepRoyale _femaleView;

        public Bed ActiveBed => _activeBed;
        private Bed[] allBeds;

        private CharacterData _currentCharacterData;
        private void Awake()
        {
            if (LevelController.Instance.GameMode == EGameMode.SleepRoyale)
            {
                allBeds = FindObjectsOfType<Bed>();
                InitBeds();
                SetTarget(_activeBed.transform);
                _activeBed.gameObject.SetActive(true);
            }

            InitCharacter();
        }

        private void InitCharacter()
        {
            var characterDatabase = Resources.Load<CharacterDatabase>("CharacterDatabase");
            _currentCharacterData = characterDatabase.CharacterDatas[SaveManager.SavedData.CurrentCharacterDataIndex];

            if (_currentCharacterData.CharacterGender == eCharacterGender.Female)
            {
                _femaleView.characterView.UpdateView(_currentCharacterData);
                _femaleView.gameObject.SetActive(true);
            }
            else
            {
                _maleView.characterView.UpdateView(_currentCharacterData);
                _maleView.gameObject.SetActive(true);
            }
        }

        private void InitBeds()
        {
            for (int i = 0; i < allBeds.Length; i++)
            {
                if (allBeds[i] != _activeBed)
                    allBeds[i].gameObject.SetActive(false);
                else
                {
                    allBeds[i].gameObject.SetActive(true);
                    allBeds[i].Activeted();
                }
            }
        }
    }
}