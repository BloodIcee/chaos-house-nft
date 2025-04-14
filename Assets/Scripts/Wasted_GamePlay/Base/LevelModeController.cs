using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModeController : MonoBehaviour
{
    [SerializeField] LevelMode[] levelModes;
    public Transform CurrentTarget { get; private set; }
    private void Awake()
    {
        SetRandomMode();
    }

    private void SetRandomMode() {
        for (int i = 0; i < levelModes.Length; i++)
        {
            levelModes[i].gameObject.SetActive(false);
        }

        var r = Random.Range(0, levelModes.Length);
        levelModes[r].gameObject.SetActive(true);
        CurrentTarget = levelModes[r].finishTarget;
    }
}
