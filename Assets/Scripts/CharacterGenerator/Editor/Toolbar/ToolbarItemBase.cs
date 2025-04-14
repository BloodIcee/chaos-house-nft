using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ChaosHouse.CharacterGenerator
{
    public abstract class ToolbarItemBase
    {
        protected List<Material> loadedMaterials;
        protected void OnlyDrawMaterialsField(List<Material> materials)
        {
            GUI.enabled = false;
            for (int i = 0; i < materials.Count; i++)
            {
                EditorGUILayout.ObjectField(materials[i], typeof(Material));
            }
            GUI.enabled = true;
        }

        protected bool DrawChangableControl(Action drawer)
        {
            EditorGUI.BeginChangeCheck();
            drawer?.Invoke();
            return EditorGUI.EndChangeCheck();
        }

        protected List<Material> LoadAsyncMaterials(string[] materials)
        {
            if (materials == null) return new List<Material>();

            List<Material> materialList = new List<Material>();

            for (int i = 0; i < materials.Length; i++)
            {
                AddressablesLoader.LoadAsset<Material>(materials[i], (AsyncOperationHandle<Material> obj) => { materialList.Add(obj.Result); });
            }

            return materialList;

        }
    }
}