using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class MaleView : CharacterView
    {
        [SerializeField] private FacialHairRootView facialHairRootView;

        public override IEnumerator UpdateCharacterData(CharacterData characterData)
        {
            yield return new WaitForSeconds(0.1f);

            clothesViewRoot.SetEnity(characterData.Clothes);
            clothesViewRoot.SetEnityMaterials(characterData.Clothes);           

            accessoriesViewRoot.SetEnity(characterData.Accessory);
            accessoriesViewRoot.SetEnityMaterials(characterData.Accessory);

            hairRootView.SetEnity(characterData.HairData);
            hairRootView.currentEntity.SetTexture(characterData.HairData.TextureName);

            bodyView.SetTexture(characterData.BodyData.TextureName);

            faceView.SetBodyPartMaterial(characterData.BodyData.MaterialNames[0], characterData.BodyData.TextureName);
            faceView.SetHairPartMaterial(characterData.HairData.MaterialNames[0], characterData.HairData.TextureName);

            facialHairRootView.SetEnity(characterData.FacialHairData);
            facialHairRootView.currentEntity?.SetTexture(characterData.HairData.TextureName);

            animator.Play(characterData.IdleAnimationName);

            UpdateBoingBones();
        }
        public override void UpdateView(CharacterData characterData)
        {
            base.UpdateView(characterData);

            ChangeFacialHair(characterData.FacialHairData);
            facialHairRootView.currentEntity.SetTexture(characterData.HairData.TextureName);
        }
        public void ChangeFacialHair(FacialHair facialHair) => facialHairRootView.SetEnity(facialHair);

        public List<string> GetAllFacialHairNames(bool checkActive = false)
        {
            var facialHair = GetPartNames<FacialHair, FacialHairRootView, FacialHairView>(facialHairRootView, checkActive);
            facialHair.Insert(0, "None");
            return facialHair;
        }

        private List<FacialHair> GetAllFacialHair(bool checkActive = false) => GetPartList<FacialHair, FacialHairRootView, FacialHairView>(facialHairRootView, checkActive);

        public FacialHair GetActiveFacialHair()
        {
            var activeList = GetAllFacialHair(true);
            if (activeList.Count > 0) return activeList[0];
            else return new FacialHair();
        }

        public string[] GetFacialHairMainMaterialNames()
        {
            var activeList = GetAllFacialHair();
            return activeList[0].MaterialNames;
        }

        public FacialHair GetCurrentEyelashes => new FacialHair(facialHairRootView.currentEntity.CharacterPartData.PartName, facialHairRootView.currentEntity.CharacterPartData.MaterialNames);
        public void ChangeEyelashes(FacialHair facialHair) => facialHairRootView.SetEnity(facialHair);
    }
}