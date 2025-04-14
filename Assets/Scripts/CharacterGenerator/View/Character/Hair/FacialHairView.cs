using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ChaosHouse
{
    public class FacialHairView : CharacterRenderEntity
    {
        public string GetTextureName()
        {
            if (renderer == null) InitComponents();

            Material material = renderer.sharedMaterials[0];
            return material.GetTexture("_MainTex").name;
        }

        public void SetTexture(string textureName)
        {
            if (renderer == null) InitComponents();

            AddressablesLoader.LoadAsset<Texture>(textureName, (AsyncOperationHandle<Texture> obj) => ApplyTexture(obj.Result));
        }

        private void ApplyTexture(Texture texture)
        {

            if (Application.isPlaying)
            {
                var materials = renderer.sharedMaterials;
                Material material = Instantiate(materials[0]);
                material.SetTexture("_MainTex", texture);
                materials[0] = material;
                renderer.sharedMaterials = materials;
            }
            else
            {
                Material material = renderer.sharedMaterials[0];
                material.SetTexture("_MainTex", texture);
                renderer.sharedMaterials[0] = material;
            }
        }
    }
}
