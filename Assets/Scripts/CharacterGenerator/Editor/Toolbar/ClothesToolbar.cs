using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChaosHouse.CharacterGenerator
{
    public class ClothesToolbar : ToolbarItemBase, IToolbarItem<Clothes>
    {
        private string _name = "Clothes";
        public Clothes CharacterPart { get; private set; }
        private MatchingDatabase _matchingDatabase;
        private CharacterView _characterView;
        public event Action onChanged;
        public string ItemName => _name;

        public ClothesToolbar(Clothes characterPart, List<Material> loadedMaterials, MatchingDatabase matchingDatabase, CharacterView characterView)
        {
            this.CharacterPart = characterPart;
            this.loadedMaterials = loadedMaterials;
            this._matchingDatabase = matchingDatabase;
            this._characterView = characterView;
        }

        public void Draw()
        {
            List<string> allClothesNames = _characterView.GetAllClothesNames();
            int selectedPartIndex = allClothesNames.FindIndex(x => x == CharacterPart.PartName);
            bool clothesChanged = DrawChangableControl(() => selectedPartIndex = EditorGUILayout.Popup(selectedPartIndex, allClothesNames.ToArray()));

            if (clothesChanged)
            {
                CharacterPart.SetName(allClothesNames[selectedPartIndex]);

                _characterView.ChangeClothes(CharacterPart);

                CharacterPart.SetMaterialNames(_characterView.GetCurrentClothes.MaterialNames);

                this.loadedMaterials = LoadAsyncMaterials(CharacterPart.MaterialNames);

            }
            var gender = _characterView is FemaleView ? eCharacterGender.Female : eCharacterGender.Male;
            var matching = _matchingDatabase.GetMatchingConfig<MatchingClothingAndMaterials>(gender);

            if (matching != null)
            {
                List<string> allClothesMaterial = matching.GetMaterialNames(CharacterPart.PartName);
                if (allClothesMaterial != null)
                {
                    int selectedMaterialIndex = allClothesMaterial.FindIndex(x => x == CharacterPart.MaterialNames[0]);
                    if (selectedMaterialIndex < 0) selectedMaterialIndex = 0;

                    bool materialChanged = DrawChangableControl(() => selectedMaterialIndex = EditorGUILayout.Popup(selectedMaterialIndex, allClothesMaterial.ToArray()));

                    CharacterPart.MaterialNames[0] = allClothesMaterial[selectedMaterialIndex];

                    if (materialChanged)
                    {
                        _characterView.ChangeClothesMaterials(CharacterPart);                        
                    }
                }

            }

            OnlyDrawMaterialsField(this.loadedMaterials);
        }
    }
}