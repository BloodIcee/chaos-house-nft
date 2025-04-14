using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SelectGameModeButton : MonoBehaviour
{
    private Button button;
   // [SerializeField] EGameMode gameMode;    
    [SerializeField] SceneField _sceneField;    

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadScene);
    }

    private void LoadScene() {
        SceneManager.LoadScene(_sceneField.SceneName);

        //if (gameMode == EGameMode.Survive)
        //    SaveManager.savedData.CountStartsMallMode++;
        //else if (gameMode == EGameMode.SleepRoyale)
        //    SaveManager.savedData.CountStartsRoyaleMode++;

      //  SaveManager.Save();
    }

}
