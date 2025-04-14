using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

namespace ChaosHouse
{
    public abstract class ViewRootBase<T> : MonoBehaviour where T : CharacterRenderEntity
    {
        protected HashSet<T> _characterRenderEntities = new HashSet<T>();

        public IReadOnlyList<T> CharacterRenderEntities => _characterRenderEntities.ToList();

        private List<Material> tempMaterials = new List<Material>();
        private int tempMaterialsCount = 0;
        public T currentEntity { get; protected set; }
        public virtual void InitRenderEntity()
        {
          
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<T>(out T enity))
                {
                    _characterRenderEntities.Add(enity);

                    if (child.gameObject.activeSelf) currentEntity = enity;
                }
                
            }            
        
        }

        
        public T GetRenderEntity(string entityName)
        {
            return _characterRenderEntities.Where(x=>x.EntityName == entityName).FirstOrDefault();
        }

        public async void SetEnity<K>(K parts) where K : CharacterPart
        {            
            InitRenderEntity();

            if (currentEntity != null) currentEntity.Hide();
         
            if (parts ==null || parts.PartName == "None" || parts.PartName == null || parts.PartName == "") return;
            
            currentEntity = (T)_characterRenderEntities.First(x => x.EntityName == parts.PartName);
            if (currentEntity == null) Debug.LogError($"{parts.PartName} not found");
            currentEntity.Show();

            await Task.Yield();


        }

        public void SetEnityMaterials<K>(K parts) where K : CharacterPart
        {
            InitRenderEntity();

            if (currentEntity == null) return;

            AddressablesLoader.LoadMaterials(parts.MaterialNames, currentEntity.SetMaterials);            


        }

    }
}
