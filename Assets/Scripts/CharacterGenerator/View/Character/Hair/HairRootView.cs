using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ChaosHouse
{
    public class HairRootView : ViewRootBase<HairView>
    {
        public override void InitRenderEntity()
        {
            base.InitRenderEntity();
            if (currentEntity == null)
            {
                currentEntity = _characterRenderEntities.First();
                currentEntity.Show();
            }

            if (!currentEntity.gameObject.activeSelf) currentEntity.Show();

        }
    }
}
