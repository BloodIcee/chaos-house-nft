using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChaosHouse.CharacterGenerator
{
    public class BodyToolbar : ToolbarItemBase, IToolbarItem<Body>
    {
        private string _name = "Body";
        public Body CharacterPart { get; private set; }
        private MatchingDatabase _matchingDatabase;
        private CharacterView _characterView;
        public event Action onChanged;
        public string ItemName => _name;

        public BodyToolbar(Body characterPart, List<Material> loadedMaterials, MatchingDatabase matchingDatabase, CharacterView characterView)
        {
            this.CharacterPart = characterPart;
            this.loadedMaterials = loadedMaterials;
            this._matchingDatabase = matchingDatabase;
            this._characterView = characterView;
        }

        public void Draw()
        {
            EditorGUILayout.LabelField(CharacterPart.PartName);

            var matchingBody = _matchingDatabase.GetMatchingConfig<MatchingBodySkinAndMaterials>();

            if (matchingBody != null)
            {
                List<string> allTextureNames = matchingBody.GetTextureNames();
                int selectedPartIndex = allTextureNames.FindIndex(x => x == CharacterPart.TextureName);

                OnlyDrawMaterialsField(this.loadedMaterials);

                bool skinBodyChanged = DrawChangableControl(() => selectedPartIndex = EditorGUILayout.Popup(selectedPartIndex, allTextureNames.ToArray()));

                if (skinBodyChanged)
                {
                    CharacterPart.SetTextureName(allTextureNames[selectedPartIndex]);
                    _characterView.ChangeBodySkin(CharacterPart);                    
                }

            }
        }
        
    }
}