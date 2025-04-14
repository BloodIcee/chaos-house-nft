using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "SuriveConfig", menuName = "Configs/SuriveConfig")]
public class SurviveConfig : ScriptableObject
{
    public float MaxMoveSpeed;
    public float DurationAcceleration;
    public float BrakingSpeed;
    public float MaxPlayerHealth;
    public float MaxBotSpeed;
}
