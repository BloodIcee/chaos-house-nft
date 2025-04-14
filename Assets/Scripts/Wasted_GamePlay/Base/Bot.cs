using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [System.Serializable]
    public abstract class Bot : Actor
    {
        [HideInInspector] public List<GameObject> collisionObj = new List<GameObject>();
        public override void Awake()
        {
            base.Awake();
            Name = LevelController.Instance.NamesConfig.GetRandomName();
            GetComponentInChildren<ActorName>().SetName(Name);
        }

        public virtual void Falling() { }

    }
}
