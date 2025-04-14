using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorName : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] bool royale;

    private Camera camera;
    private Vector3 target;

    private void Start()
    {
        camera = Camera.main;
        EventManager.Subscribe(EEventsName.LevelComplite, HandleLevelComplite);
    }

    private void HandleLevelComplite(object o) {
        gameObject.SetActive(false);
    }

    public void SetName(string newName) {
        nameText.text = newName;
    }

    private void Update()
    {

        target = camera.transform.position;
        if (royale)
        {
            target.z *= -1;
            target.x *= -1;
            nameText.transform.LookAt(target, Vector3.up);
        }
        else
        {
            target.y = nameText.transform.position.y;


            nameText.transform.LookAt(target, Vector3.up);
            var rotation = nameText.transform.rotation;
            rotation.y = 0;
            nameText.transform.rotation = rotation;
        }

    }

      

    
}
