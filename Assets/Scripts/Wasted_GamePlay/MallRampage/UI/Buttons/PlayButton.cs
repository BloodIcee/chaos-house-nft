using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : ButtonBase
{
    protected override void ButtonFunc()
    {
        EventManager.OnEvent(EEventsName.LevelStart);       
    }
}
