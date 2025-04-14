using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public abstract class ButtonBase : MonoBehaviour
{
    private Button _button;
    protected virtual void Awake() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonFunc);
    }

    protected abstract void ButtonFunc();
}
