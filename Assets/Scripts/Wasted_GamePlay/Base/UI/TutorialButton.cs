using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    [SerializeField] float delayStart;
    float timer = 0f;

    private void Update()
    {
        if (Input.GetMouseButton(0)) {
            timer += Time.deltaTime;
            if (timer > delayStart)
            {
                EventManager.OnEvent(EEventsName.LevelStart);
                gameObject.SetActive(false);
            }
        }
    }

}
