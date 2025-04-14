using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class CanvasManager : MonoBehaviour
    {
        private List<GameWindow> gamesWindows = new List<GameWindow>();
        private GameWindow currentWindow;
        [SerializeField] EWindowType defaultWindow;
        private void Start()
        {
            InitWindows();

            if (LevelController.Instance != null) InitGameWindow();            

            SetCurrentWindow(defaultWindow);

            EventManager.Subscribe(EEventsName.LevelComplite, HandleLevelComplite);
            EventManager.Subscribe(EEventsName.LevelLost, HandleLevelLost);
        }

        private void HandleLevelLost(object o)
        {
            StartCoroutine(SetActiveWithDelay(1f, EWindowType.Result));
        }

        private void HandleLevelComplite(object o)
        {
            StartCoroutine(SetActiveWithDelay(1f, EWindowType.Result));
        }

        private IEnumerator SetActiveWithDelay(float delay, EWindowType windowType)
        {
            yield return new WaitForSeconds(delay);
            SetCurrentWindow(windowType);
        }
        private void InitWindows()
        {
            foreach (Transform child in transform)
            {
                var window = child.GetComponent<GameWindow>();
                if (window != null)
                {
                    window.Hide();
                    gamesWindows.Add(window);
                }
            }
        }

        private void SetCurrentWindow(EWindowType windowType)
        {
            if (currentWindow != null) currentWindow.Hide();
            currentWindow = gamesWindows.Find(x => x.WindowType == windowType);
            if (currentWindow != null) currentWindow.Show();
        }

        private void  InitGameWindow() {
            switch (LevelController.Instance.GameMode)
            {
                case EGameMode.Menu:
                    defaultWindow = EWindowType.Menu;
                    break;
                case EGameMode.Survive:                    
                    break;
                case EGameMode.SleepRoyale:
                    defaultWindow = EWindowType.RoyaleMode;
                    break;
                case EGameMode.RunToFindItem:
                    defaultWindow = EWindowType.FindItems;
                    break;
                default:
                    defaultWindow = EWindowType.Menu;
                    break;
            }
        }
    }
}