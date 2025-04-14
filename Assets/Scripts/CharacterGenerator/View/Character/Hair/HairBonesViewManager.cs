using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class HairBonesViewManager : MonoBehaviour
    {
        [field: SerializeField] public List<HairBonesView> hairBonesViews = new List<HairBonesView>();
        
        public HairBonesView GetHairBonesView(string hairName)
        {
            var hairBonesView = hairBonesViews.Find(x => x.HairName == hairName);
            return hairBonesView;
        }

    }
}
