using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMode : MonoBehaviour
{
    public Transform finishTarget { get; protected set; }

    protected void SetTarget(Transform t) {
        finishTarget = t;
    }
}
