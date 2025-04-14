using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public static class SaveManager
    {
        private static SavedData _savedData;
        public static SavedData SavedData
        {
            get
            {
                if (_savedData == null)
                {
                    _savedData = new SavedData();
                    _savedData.Init();
                }
                return _savedData;
            }
        }

    }

    public class SavedData
    {
        private static string savedName = "ChaosHouseData";

       [SerializeField] private string _playerName = "Player Name";  
       [SerializeField] private int  _currentCharacterDataIndex = -1;
        public SavedData()
        {

        }

        public void Init()
        {
            if (PlayerPrefs.HasKey(savedName))
            {
                var jsonStr = PlayerPrefs.GetString(savedName);
                SavedData savedData = JsonUtility.FromJson<SavedData>(jsonStr);
                this._playerName = savedData.PlayerName;
                this._currentCharacterDataIndex = savedData.CurrentCharacterDataIndex;
            }
        }

        public void Save()
        {
            var savedStr = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(savedName, savedStr);
           
        }
        public void ChangePlayerName(string playerName)
        {
            this._playerName = playerName;
            Save();
        }

        public void ChangeCurrentCharacterData(int index)
        {
            this._currentCharacterDataIndex = index;
            Save();
        } 

        public string PlayerName => _playerName;
        public int CurrentCharacterDataIndex => _currentCharacterDataIndex;                

    }
}