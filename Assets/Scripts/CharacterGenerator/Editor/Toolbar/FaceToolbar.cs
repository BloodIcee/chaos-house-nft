using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChaosHouse.CharacterGenerator
{
    public class FaceToolbar: ToolbarItemBase, IToolbarItem<Face>
    {
        private string _name = "Face";
        public Face CharacterPart { get; private set; }

        public event Action onChanged;

        public string ItemName => _name;

        
        public FaceToolbar(Face characterPart, List<Material> loadedMaterials)
        {
            CharacterPart = characterPart;
            this.loadedMaterials = loadedMaterials;
        }

        public void Draw()
        {
            EditorGUILayout.LabelField(CharacterPart.PartName);

            OnlyDrawMaterialsField(this.loadedMaterials);
        }
    }
}