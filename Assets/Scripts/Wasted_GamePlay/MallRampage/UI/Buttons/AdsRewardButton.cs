using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsRewardButton : ButtonBase
{
    protected override void ButtonFunc()
    {
        EventManager.OnEvent(EEventsName.LevelRestart);
    }
}
