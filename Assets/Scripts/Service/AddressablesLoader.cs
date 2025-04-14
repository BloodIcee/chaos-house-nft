using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ChaosHouse
{
    public static class AddressablesLoader
    {

        public static event Action LoadEnded;
        public static void LoadAsset<T>(string name, Action<AsyncOperationHandle<T>> OnLoadDependencyDone) where T : UnityEngine.Object
        {
            AsyncOperationHandle<T> asyncOperationHandle = Addressables.LoadAssetAsync<T>(name);
            asyncOperationHandle.Completed += (AsyncOperationHandle<T> obj) => { LoadEnded?.Invoke(); OnLoadDependencyDone?.Invoke(obj); };           
            
        }

        public static void LoadMaterials(string[] materials, Action<Material []> actionOnComplite)
        {
            List<Material> materialList = new List<Material>();

            for (int i = 0; i < materials.Length; i++)
            {
                if (i == materials.Length - 1) AddressablesLoader.LoadAsset<Material>(materials[i], (AsyncOperationHandle<Material> obj) => { materialList.Add(obj.Result); actionOnComplite?.Invoke(materialList.ToArray()); LoadEnded?.Invoke(); });
                else AddressablesLoader.LoadAsset<Material>(materials[i], (AsyncOperationHandle<Material> obj) => { materialList.Add(obj.Result); LoadEnded?.Invoke(); });
                
            }          

        }
    }
}

