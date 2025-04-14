using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ChaosHouse {
    public class CharacterSkinManager : MonoBehaviour
    {
        [SerializeField] private MaleView _maleView;
        [SerializeField] private FemaleView _femaleView;

        private CharacterDatabase _characterDatabase;
        private List<CharacterData> _charactedDataInVirtualWallet = new List<CharacterData>();
        private List<CharacterData> _allCharactedData = new List<CharacterData>();

        private CharacterData _currentCharacterData;
        private int _indexCurrentCharactedData = 0;

        public CharacterData CurrentCharacterData=> _currentCharacterData;
        public int IndexCurrentCharactedData =>_indexCurrentCharactedData;

        private void Awake()
        {
            _characterDatabase = Resources.Load<CharacterDatabase>("CharacterDatabase");
            _allCharactedData = _characterDatabase.CharacterDatas as List<CharacterData>;

            if (LevelController.Instance.FreeAllSkins) OpenAllCharacterSkins();

            InitSkins();

        }

        private void OpenAllCharacterSkins()
        {
            for (int i = 0; i < _allCharactedData.Count; i++)
            {
                VirtualWallet.WalletData.AddNewCharacterData(_allCharactedData[i]);
            }
        }
        private void InitSkins()
        {
            _currentCharacterData = _allCharactedData[_indexCurrentCharactedData];
            if (SaveManager.SavedData.CurrentCharacterDataIndex == -1) SaveManager.SavedData.ChangeCurrentCharacterData(_indexCurrentCharactedData);
            else
            {
                _indexCurrentCharactedData = SaveManager.SavedData.CurrentCharacterDataIndex;
                _currentCharacterData = _allCharactedData[_indexCurrentCharactedData];
            }

            //SaveManager.SavedData.ChangeCurrentCharacterData(_indexCurrentCharactedData);
            UpdateCharacter();
        }

        private void UpdateCharacter()
        {
            bool isMale = CurrentCharacterData.CharacterGender == eCharacterGender.Male;

            if (!isMale)
            {
                _femaleView.UpdateView(_currentCharacterData);                
            }
            else
            {
                _maleView.UpdateView(_currentCharacterData);                
            }

            _maleView.gameObject.SetActive(isMale);
            _femaleView.gameObject.SetActive(!isMale);

            SaveManager.SavedData.ChangeCurrentCharacterData(_indexCurrentCharactedData);
        }
        

        public void SetNextSkin() {

            if (_indexCurrentCharactedData < _allCharactedData.Count - 1) _indexCurrentCharactedData++;
            else _indexCurrentCharactedData = 0;

            
            _currentCharacterData = _allCharactedData[_indexCurrentCharactedData];
            UpdateCharacter();
        }

        public void SetPreviousSkin()
        {
            if (_indexCurrentCharactedData > 0) _indexCurrentCharactedData--;
            else _indexCurrentCharactedData = _allCharactedData.Count - 1;

            _currentCharacterData = _allCharactedData[_indexCurrentCharactedData];
            UpdateCharacter();
        }

    }
}