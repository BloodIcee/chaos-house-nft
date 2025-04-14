using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using System.Linq;

[System.Serializable]
public class SceneField
{
    [SerializeField] private string _sceneName;
    public string SceneName=> _sceneName;
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldDrawer:PropertyDrawer
{
    private string[] _sceneNames;
   
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {        
        if (_sceneNames == null) InitSceneNames();

        var sceneNameProp = property.FindPropertyRelative("_sceneName");

        int selectedSceneIndex = _sceneNames.ToList().FindIndex(x=>x == sceneNameProp.stringValue);
        if (selectedSceneIndex == -1) selectedSceneIndex = 0;
        sceneNameProp.stringValue = _sceneNames[selectedSceneIndex];

        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var sceneRect = new Rect(position.x, position.y, position.width, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        selectedSceneIndex = EditorGUI.Popup(sceneRect, selectedSceneIndex, _sceneNames);
        sceneNameProp.stringValue = _sceneNames[selectedSceneIndex];
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();

        property.serializedObject.ApplyModifiedProperties();
    }

    private void InitSceneNames() 
    {
        _sceneNames = EditorBuildSettings.scenes
              .Where(scene => scene.enabled)
              .Select(scene => scene.path)
              .ToArray();

        for (int i = 0; i < _sceneNames.Length; i++)
        {
            var lastindex = _sceneNames[i].LastIndexOf("/");
            _sceneNames[i] = _sceneNames[i].Remove(0, lastindex+1);
            _sceneNames[i] = _sceneNames[i].Replace(".unity","");
        }
    }
}
#endif