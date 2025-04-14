using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NamesConfig", menuName = "Configs/NamesConfig")]
public class NamesConfig : ScriptableObject
{
    public List<string> ActorNames = new List<string>();

    public string GetRandomName() {
        var r = Random.Range(0, ActorNames.Count);
        return ActorNames[r];
    }
}
