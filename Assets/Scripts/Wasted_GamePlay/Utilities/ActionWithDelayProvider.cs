using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWithDelayProvider : MonoBehaviour
{
    public static ActionWithDelayProvider Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void InvokeAction(Action action, float delay)
    {
        StartCoroutine(StartActionWithDelay(action, delay));
    }

    private IEnumerator StartActionWithDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
