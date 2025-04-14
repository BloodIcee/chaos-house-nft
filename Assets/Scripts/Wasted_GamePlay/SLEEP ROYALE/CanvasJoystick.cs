using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasJoystick : MonoBehaviour
{
    [SerializeField] GameObject[] childs;
    private void Start()
    {
        EventManager.Subscribe(EEventsName.LevelStart, HandleLevelStart);
        EventManager.Subscribe(EEventsName.LevelComplite, HandleLevelComplite);
        EventManager.Subscribe(EEventsName.LevelLost, HandleLevelComplite);
        SetState(false);
    }

    private void HandleLevelStart(object o) {
        SetState(true);
    }

    private void HandleLevelComplite(object o)
    {
        SetState(false);
    }

    private void SetState(bool state) {
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].SetActive(state);
        }
    }
}
