using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosHouse
{
    public class InputPlayerName : MonoBehaviour
    {
        private InputField inputField;
        private void Start()
        {
            inputField = GetComponent<InputField>();
            inputField.text = SaveManager.SavedData.PlayerName;
        }
        public void SetPlayerName(string newName)
        {
            SaveManager.SavedData.ChangePlayerName(newName);
        }
    }
}