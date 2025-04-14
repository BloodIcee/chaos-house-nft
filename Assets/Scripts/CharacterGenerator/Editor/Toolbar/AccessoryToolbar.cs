using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChaosHouse.CharacterGenerator
{
    public class AccessoryToolbar:ToolbarItemBase, IToolbarItem<Accessory>
    {
        private string _name = "Accessory";
        public Accessory CharacterPart { get; private set; }
        private MatchingDatabase _matchingDatabase;
        private CharacterView _characterView;
        public event Action onChanged;
        public string ItemName => _name;

        public AccessoryToolbar(Accessory characterPart, List<Material> loadedMaterials, MatchingDatabase matchingDatabase, CharacterView characterView)
        {
            this.CharacterPart = characterPart;
            this.loadedMaterials = loadedMaterials;
            this._matchingDatabase = matchingDatabase;
            this._characterView = characterView;
        }

        public void Draw()
        {
            List<string> allAccessorieNames = _characterView.GetAllAccessoriesNames();

            int selectedPartIndex = allAccessorieNames.FindIndex(x => x == CharacterPart.PartName);
            if (selectedPartIndex < 0) selectedPartIndex = 0;

            bool accessoryChanged = DrawChangableControl(() => selectedPartIndex = EditorGUILayout.Popup(selectedPartIndex, allAccessorieNames.ToArray()));

            if (accessoryChanged)
            {
                CharacterPart.SetName(allAccessorieNames[selectedPartIndex]);
                _characterView.ChangeAccessory(CharacterPart);

                if (selectedPartIndex != 0)
                {
                    CharacterPart.SetMaterialNames(_characterView.GetCurrentAccessory.MaterialNames);                    
                }

                onChanged?.Invoke();
            }

            var gender = _characterView is FemaleView ? eCharacterGender.Female : eCharacterGender.Male;
            var matchingAccessories = _matchingDatabase.GetMatchingConfig<MatchingAccessoriesMaterials>(gender);

            if (matchingAccessories != null)
            {
                List<string> accessoriesMaterialList = matchingAccessories.GetMaterialNames();
                int selectedAccessorisMaterialIndex = accessoriesMaterialList.FindIndex(x => x == CharacterPart.MaterialNames[0]);
                bool accessorisMaterialChanged = DrawChangableControl(() => selectedAccessorisMaterialIndex = EditorGUILayout.Popup(selectedAccessorisMaterialIndex, accessoriesMaterialList.ToArray()));

                if (accessorisMaterialChanged)
                {
                    CharacterPart.MaterialNames[0] = accessoriesMaterialList[selectedAccessorisMaterialIndex];
                    _characterView.ChangeAccessoryMaterial(CharacterPart);
                    
                    onChanged?.Invoke();
                }
            }

            OnlyDrawMaterialsField(this.loadedMaterials);
        }
    }
}