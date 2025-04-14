using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChaosHouse.CharacterGenerator
{
    public class EyelashesToolbar : ToolbarItemBase, IToolbarItem<Eyelashes>
    {
        private string _name = "Eyelashes";
        private MatchingDatabase _matchingDatabase;
        private FemaleView _characterView;

        public Eyelashes CharacterPart { get; private set; }
        public string ItemName => _name;

        public event Action onChanged;

        public EyelashesToolbar(Eyelashes characterPart, List<Material> loadedMaterials, MatchingDatabase matchingDatabase, CharacterView characterView)
        {
            this.CharacterPart = characterPart;
            this.loadedMaterials = loadedMaterials;
            this._matchingDatabase = matchingDatabase;
            this._characterView = characterView as FemaleView;
        }

        public void Draw()
        {
            List<string> allEyelashesNames = _characterView.GetAllEyelashesNames();
            int selectedPartIndex = allEyelashesNames.FindIndex(x => x == CharacterPart.PartName);
            if (selectedPartIndex < 0) selectedPartIndex = 0;

            bool eyelashesChanged = DrawChangableControl(() => selectedPartIndex = EditorGUILayout.Popup(selectedPartIndex, allEyelashesNames.ToArray()));

            if (eyelashesChanged)
            {
                CharacterPart.SetName(allEyelashesNames[selectedPartIndex]);
                _characterView.ChangeEyelashes(CharacterPart);

                if (selectedPartIndex != 0)
                {
                    CharacterPart.SetMaterialNames(_characterView.GetCurrentEyelashes.MaterialNames);
                    this.loadedMaterials = LoadAsyncMaterials(CharacterPart.MaterialNames);
                }
            }

            OnlyDrawMaterialsField(this.loadedMaterials);
        }

    }
}