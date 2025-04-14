using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class FemaleView : CharacterView
    {
        [SerializeField] private EyelashesRootView eyelashesRootView;

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

            animator.Play(characterData.IdleAnimationName);
            eyelashesRootView.SetEnity(characterData.EyelashesData);

            UpdateBoingBones();
        }

        public override void UpdateView(CharacterData characterData)
        {
            base.UpdateView(characterData);

            ChangeEyelashes(characterData.EyelashesData);
        }
        private List<Eyelashes> GetAllEyelashes(bool checkActive = false) => GetPartList<Eyelashes, EyelashesRootView, EyelashesView>(eyelashesRootView, checkActive);
        public List<string> GetAllEyelashesNames(bool checkActive = false)
        {
            var eyeleshasNames = GetPartNames<Eyelashes, EyelashesRootView, EyelashesView>(eyelashesRootView, checkActive);
            eyeleshasNames.Insert(0, "None");
            return eyeleshasNames;
        }
        public Eyelashes GetActiveEyelashes()
        {
            var activeList = GetAllEyelashes(true);
            if (activeList.Count > 0) return activeList[0];
            else return new Eyelashes();
        }

        public string[] GetEyelashesMainMaterialNames()
        {
            var activeList = GetAllEyelashes();
            return activeList[0].MaterialNames;
        }

        public Eyelashes GetCurrentEyelashes => new Eyelashes(eyelashesRootView.currentEntity.CharacterPartData.PartName, eyelashesRootView.currentEntity.CharacterPartData.MaterialNames);
        public void ChangeEyelashes(Eyelashes eyelashes) => eyelashesRootView.SetEnity(eyelashes);
    }
}
