using System.Linq;

namespace ChaosHouse
{
    public class ClothesViewRoot : ViewRootBase<ClothesView>
    {
        public override void InitRenderEntity()
        {
            base.InitRenderEntity();
            if (currentEntity == null)
            {
                currentEntity = _characterRenderEntities.First();
                currentEntity.Show();
            }

            if(!currentEntity.gameObject.activeSelf) currentEntity.Show();

        }
    }
}
