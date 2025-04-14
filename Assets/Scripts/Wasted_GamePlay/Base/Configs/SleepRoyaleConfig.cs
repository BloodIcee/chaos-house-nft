using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "SleepRoyaleConfig", menuName = "Configs/SleepRoyaleConfig")]
public class SleepRoyaleConfig : ScriptableObject
{
    public float MaxPlayerMovementSpeed;
    public float PlayerRotationSpeed;
    
}
