using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : ButtonBase
{
    protected override void ButtonFunc()
    {
        EventManager.OnEvent(EEventsName.LevelRestart);
    }
}
