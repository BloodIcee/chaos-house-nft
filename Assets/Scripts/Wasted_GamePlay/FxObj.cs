using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxObj : MonoBehaviour
{
    [SerializeField] bool destroyed;
    [SerializeField] float delayBeforeDestroy;

    private void Start()
    {
        if (destroyed) {
            Destroy(this.gameObject, delayBeforeDestroy);       
        }
    }
     
}
