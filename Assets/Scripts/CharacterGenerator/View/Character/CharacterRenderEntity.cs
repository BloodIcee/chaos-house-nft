using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ChaosHouse
{
    [RequireComponent(typeof(Renderer))]
    public abstract class CharacterRenderEntity : MonoBehaviour
    {
        protected Renderer renderer;

        public string EntityName => gameObject.name;
        public CharacterPart CharacterPartData { get; private set; }
        protected void InitComponents()
        {
            if (TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinnedMeshRenderer))
            {
                renderer = skinnedMeshRenderer;
            }
            else if (TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
            {
                renderer = meshRenderer;
            }
            else throw new MissingReferenceException("Renderer = null");

            CharacterPartData = new CharacterPart(EntityName, GetMaterialNames());
            
        }

        public void SetAsyncMaterials<K>(K parts) where K : CharacterPart
        {
            if (renderer == null) InitComponents();

           AddressablesLoader.LoadMaterials(parts.MaterialNames, SetMaterials);    

        }

        public virtual void SetMaterials(Material[] materials)
        {
            if (renderer == null) InitComponents();

            renderer.sharedMaterials = materials;
        }

        public string[] GetMaterialNames()
        {
            if (renderer == null) InitComponents();

            string[] materialNames = new string[renderer.sharedMaterials.Length];

            for (int i = 0; i < materialNames.Length; i++)
            {
                materialNames[i] = renderer.sharedMaterials[i].name;
            }

            return materialNames;
        }
        
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);


    }
}
