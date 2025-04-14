using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
#endif
namespace ChaosHouse
{
    public abstract class CharacterView : MonoBehaviour
    {
        [SerializeField] protected CharactedDataField charactedDataField;
        [SerializeField] protected ClothesViewRoot clothesViewRoot;
        [SerializeField] protected AccessoriesViewRoot accessoriesViewRoot;    
        [SerializeField] protected HairRootView hairRootView;
        [SerializeField] protected HairBonesViewManager hairBonesViewManager;
        [SerializeField] protected BodyView bodyView;
        [SerializeField] protected FaceView faceView;
        [SerializeField] protected Animator animator;

        [SerializeField]  private BoingKit.BoingBones boingBones;
        
        [SerializeField] private bool useSelectedCharacterData;
        private CharacterData _characterData;

        public  HairBonesViewManager HairBonesViewController => hairBonesViewManager;
        
        private void Awake()
        {
            CharacterDatabase characterDatabase = Resources.Load<CharacterDatabase>("CharacterDatabase");

          
                _characterData = characterDatabase.GetCharacterData(charactedDataField.CharacterDataName);

            if (useSelectedCharacterData) StartCoroutine(UpdateCharacterData(_characterData));
            
        }

        public abstract IEnumerator UpdateCharacterData(CharacterData characterData);

        protected void UpdateBoingBones()
        {
            MatchingDatabase matchingDatabase = Resources.Load<MatchingDatabase>("MatchingDatabase");
            
            var hairBonesMatching = matchingDatabase.GetMatchingConfig<MatchingHairAndBones>(_characterData.CharacterGender);
            HairAndBones hairAndBones = hairBonesMatching.GetHairAndBones(_characterData.HairData.PartName);

            if (hairAndBones == null) return;

            List<Transform> bonesList = hairBonesViewManager.GetHairBonesView(_characterData.HairData.PartName).GetBonesTransforms(hairAndBones.BonesNames);

            boingBones.BoneChains = new BoingKit.BoingBones.Chain[bonesList.Count];

            for (int i = 0; i < bonesList.Count; i++)
            {
                boingBones.BoneChains[i] = new BoingKit.BoingBones.Chain();
                boingBones.BoneChains[i].Root = bonesList[i];
                boingBones.BoneChains[i].Exclusion = new Transform[0];
                boingBones.BoneChains[i].PoseStiffnessCurveType = BoingKit.BoingBones.Chain.CurveType.RootOneTailHalf;
               // boingBones.Reboot();
            }
        }

#if UNITY_EDITOR

        public AnimationClip GetCurentAnimationClip(string animName) => GetAnimationClips().ToList().Find(x => x.name == animName);

        public AnimationClip[] GetAnimationClips()
        {
            return animator.runtimeAnimatorController.animationClips;
        }
        
        public IEnumerator PlayAnimInEditor(GameObject go, int animationClipIndex, float timeDuration)
        {
            if (animator == null) yield return null;
            float timer = 0;
            float timeStep = 0.1f;

            AnimationClip animationClip = GetAnimationClips()[animationClipIndex];
            while (timer < timeDuration)
            {
                yield return new EditorWaitForSeconds(timeStep);
                Debug.Log(timer);
                AnimationMode.SampleAnimationClip(go, animationClip, timer);
                EditorApplication.update();
                timer += timeStep;
                
            }
        }

#endif
    public Body GetBody() => new Body(bodyView.EntityName, bodyView.GetMaterialNames(), bodyView.GetTextureName());
        public Face GetFace() => new Face(faceView.EntityName, faceView.GetMaterialNames());
        public Hair GetActiveHair() => GetAllHair(true)[0];
        public Clothes GetActiveClothes()
        {
            var activeList = GetAllClothes(true);
            if (activeList.Count > 0) return activeList[0];
            else return new Clothes();
        }  
   
        public Clothes GetCurrentClothes => new Clothes( clothesViewRoot.currentEntity.CharacterPartData.PartName, clothesViewRoot.currentEntity.CharacterPartData.MaterialNames);
        public Accessory GetCurrentAccessory => new Accessory(accessoriesViewRoot.currentEntity.CharacterPartData.PartName, accessoriesViewRoot.currentEntity.CharacterPartData.MaterialNames);
        public Hair GetCurrentHair => new Hair(hairRootView.currentEntity.CharacterPartData.PartName, hairRootView.currentEntity.CharacterPartData.MaterialNames, hairRootView.currentEntity.GetTextureName());
        
        public void ChangeClothes(Clothes clothes) => clothesViewRoot.SetEnity(clothes);   
        public void ChangeClothesMaterials(Clothes clothes) => clothesViewRoot.SetEnityMaterials(clothes);  
        public void ChangeHairMaterials(Hair hair) => hairRootView.SetEnityMaterials(hair);
        public void ChangeHair(Hair hair) => hairRootView.SetEnity(hair);
        public void ChangedBrows(Hair hair) => faceView.SetHairPartMaterial(hair.MaterialNames[0], hair.TextureName);
        public void ChangeAccessory(Accessory accessory) => accessoriesViewRoot.SetEnity(accessory);
        public void ChangeAccessoryMaterial(Accessory accessory)
        {
            if (accessory.PartName != "None") accessoriesViewRoot.currentEntity.SetAsyncMaterials(accessory);
        }
        
        public void ChangeBodySkin(Body body) => bodyView.SetTexture(body.TextureName);
        public void ChangeHairTexture(Hair hair) => hairRootView.currentEntity.SetTexture(hair.TextureName);

        public virtual void UpdateView(CharacterData characterData)
        {
            this._characterData = characterData;            

            ChangeClothes(characterData.Clothes);
            ChangeClothesMaterials(characterData.Clothes);

            ChangeBodySkin(characterData.BodyData);

            ChangeHair(characterData.HairData);
            ChangeHairMaterials(characterData.HairData);
            ChangeHairTexture(characterData.HairData);

            ChangeAccessory(characterData.Accessory);
            ChangeAccessoryMaterial(characterData.Accessory);

            ChangedBrows(characterData.HairData);

            animator.Play(_characterData.IdleAnimationName);

            UpdateBoingBones();
        }
        public Accessory GetActiveAccessory()
        {
            var activeList = GetAllAccessories(true);
            if (activeList.Count > 0) return activeList[0];
            else return new Accessory();
        }


        public List<Clothes> GetAllClothes(bool checkActive = false) => GetPartList<Clothes, ClothesViewRoot, ClothesView>(clothesViewRoot, checkActive);
        public List<string> GetAllClothesNames(bool checkActive = false) => GetPartNames<Clothes, ClothesViewRoot, ClothesView>(clothesViewRoot, checkActive);        

        private List<Accessory> GetAllAccessories(bool checkActive = false) => GetPartList<Accessory, AccessoriesViewRoot,  AccessoryView>(accessoriesViewRoot, checkActive);
        public List<string> GetAllAccessoriesNames(bool checkActive = false)
        {
            var accessories = GetPartNames<Accessory, AccessoriesViewRoot, AccessoryView>(accessoriesViewRoot, checkActive);
            accessories.Insert(0, "None");
            return accessories;
        }

        public List<Hair> GetAllHair(bool checkActive = false) => GetPartList<Hair, HairRootView, HairView>(hairRootView, checkActive);
        public List<string> GetAllHairNames(bool checkActive = false) => GetPartNames<Hair, HairRootView, HairView>(hairRootView, checkActive);

        protected List<T> GetPartList<T,K,P>(K viewRootBase, bool checkActive = false) where P :CharacterRenderEntity where K : ViewRootBase<P> where T : CharacterPart, new ()
        {
            List<T> partList = new List<T>();

            viewRootBase.InitRenderEntity();

            foreach (var part in viewRootBase.CharacterRenderEntities)
            {
                if (checkActive && !part.gameObject.activeSelf) continue;

                var characterPart = new T();
                characterPart.SetName(part.EntityName);
                characterPart.SetMaterialNames(part.GetMaterialNames());

                partList.Add((T)characterPart);
            }

            return partList;
        }
        protected List<string> GetPartNames<T, K, P>(K viewRootBase, bool checkActive = false) where P : CharacterRenderEntity where K : ViewRootBase<P> where T : CharacterPart, new()
        {
            List<string> partList = new List<string>();

            viewRootBase.InitRenderEntity();

            foreach (var part in viewRootBase.CharacterRenderEntities)
            {
                if (checkActive && !part.gameObject.activeSelf) continue;

                var characterPart = new T();
                characterPart.SetName(part.EntityName);
                characterPart.SetMaterialNames(part.GetMaterialNames());

                partList.Add(characterPart.PartName);
            }

            return partList;
        }

        public void UpdateClothes(Clothes clothes)
        {
           clothesViewRoot.SetEnity<Clothes>(clothes);
        }
    }
}
