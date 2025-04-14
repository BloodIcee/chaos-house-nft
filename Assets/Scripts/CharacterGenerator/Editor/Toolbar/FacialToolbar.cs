using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChaosHouse.CharacterGenerator
{
    public class FacialToolbar : ToolbarItemBase, IToolbarItem<FacialHair>
    {
        private string _name = "Facial Hair";
        public FacialHair CharacterPart { get; private set; }
        private MatchingDatabase _matchingDatabase;
        private MaleView _characterView;
        public event Action onChanged;
        public string ItemName => _name;

        public FacialToolbar(FacialHair characterPart, List<Material> loadedMaterials, MatchingDatabase matchingDatabase, CharacterView characterView)
        {
            this.CharacterPart = characterPart;
            this.loadedMaterials = loadedMaterials;
            this._matchingDatabase = matchingDatabase;
            this._characterView = characterView as MaleView;

            
        }

        public void Draw()
        {
            List<string> allFacialHairNames = _characterView.GetAllFacialHairNames();

            if (CharacterPart == null) CharacterPart = new FacialHair(allFacialHairNames[0], _characterView.GetFacialHairMainMaterialNames());

            int selectedPartIndex = allFacialHairNames.FindIndex(x => x == CharacterPart.PartName);
            if (selectedPartIndex < 0) selectedPartIndex = 0;

            bool facialHairChanged = DrawChangableControl(() => selectedPartIndex = EditorGUILayout.Popup(selectedPartIndex, allFacialHairNames.ToArray()));

            if (facialHairChanged)
            {
                CharacterPart.SetName(allFacialHairNames[selectedPartIndex]);
                _characterView.ChangeFacialHair(CharacterPart);

                if (selectedPartIndex != 0)
                {
                    CharacterPart.SetMaterialNames(_characterView.GetCurrentAccessory.MaterialNames);
                }

                onChanged?.Invoke();
            }

            OnlyDrawMaterialsField(this.loadedMaterials);
        }
    }
}