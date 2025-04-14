using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChaosHouse.CharacterGenerator
{
    public class HairToolbar : ToolbarItemBase, IToolbarItem<Hair>
    {
        private string _name = "Hair";
        private MatchingDatabase _matchingDatabase;
        private CharacterView _characterView;

        public Hair CharacterPart { get; private set; }
        public string ItemName => _name;

        public event Action onChanged;

        public HairToolbar(Hair characterPart, List<Material> loadedMaterials, MatchingDatabase matchingDatabase, CharacterView characterView)
        {
            this.CharacterPart = characterPart;
            this.loadedMaterials = loadedMaterials;
            this._matchingDatabase = matchingDatabase;
            this._characterView = characterView;
        }

        public void Draw()
        {
            List<string> allHairNames = _characterView.GetAllHairNames();
            int selectedPartIndex = allHairNames.FindIndex(x => x == CharacterPart.PartName);
            bool hairChanged = DrawChangableControl(() => selectedPartIndex = EditorGUILayout.Popup(selectedPartIndex, allHairNames.ToArray()));

            if (hairChanged)
            {
                CharacterPart.SetName(allHairNames[selectedPartIndex]);

                _characterView.ChangeHair(CharacterPart);

                CharacterPart.SetMaterialNames(_characterView.GetCurrentHair.MaterialNames);

                this.loadedMaterials = LoadAsyncMaterials(CharacterPart.MaterialNames);

                onChanged?.Invoke();
            }


            OnlyDrawMaterialsField(this.loadedMaterials);

            var matchingHair = _matchingDatabase.GetMatchingConfig<MatchingHairAndTexture>();

            if (matchingHair != null)
            {
                List<string> allHairTextureNames = matchingHair.GetTextureNames();
                int selectedTextureIndex = allHairTextureNames.FindIndex(x => x == CharacterPart.TextureName);
                bool hairTextureChanged = DrawChangableControl(() => selectedTextureIndex = EditorGUILayout.Popup(selectedTextureIndex, allHairTextureNames.ToArray()));


                if (hairTextureChanged)
                {
                    CharacterPart.SetTextureName(allHairTextureNames[selectedTextureIndex]);
                    _characterView.ChangeHairTexture(CharacterPart);

                    onChanged?.Invoke();
                }

            }
        }
    }
}