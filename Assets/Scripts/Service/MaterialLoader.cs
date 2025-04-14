using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChaosHouse
{
    public class MaterialLoader
    {
        public List<Material> materialList { get; private set; } = new List<Material>();
        public event Action onLoaded;
        public MaterialLoader(string [] materialsName)
        {
            AddressablesLoader.LoadMaterials(materialsName, GetMaterials);
        }
        protected void GetMaterials(Material[] materials)
        {
            materialList = materials.ToList();
            onLoaded?.Invoke();
        }

    }
}