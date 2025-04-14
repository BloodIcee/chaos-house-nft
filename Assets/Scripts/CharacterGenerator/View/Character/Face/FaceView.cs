using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ChaosHouse
{
    public class FaceView : CharacterRenderEntity
    {        
        public void SetBodyPartMaterial(string bodyMaterialName, string TextureName)
        {
            if (renderer == null) InitComponents();

            AddressablesLoader.LoadAsset<Texture>(TextureName, (AsyncOperationHandle<Texture> obj) => ApplyTexture(obj.Result, bodyMaterialName));
        }
        public void SetHairPartMaterial(string hairMaterialName, string TextureName)
        {
            if (renderer == null) InitComponents();

            AddressablesLoader.LoadAsset<Texture>(TextureName, (AsyncOperationHandle<Texture> obj) => ApplyTexture(obj.Result, hairMaterialName));
        }
        private void ApplyTexture(Texture texture, string matName)
        {

            if (Application.isPlaying)
            {
                var materials = renderer.sharedMaterials;
                int matIndex = -1;

                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i].name == matName || materials[i].name == matName + "(Clone)") matIndex = i;
                }

                Material material = Instantiate(materials[matIndex]);
                material.name = matName;
                material.SetTexture("_MainTex", texture);
                materials[matIndex] = material;
                renderer.sharedMaterials = materials;
            }
        }
    }
}
