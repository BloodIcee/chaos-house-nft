using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class DoorSurviveMode : MonoBehaviour
    {
        [SerializeField] Transform fx;

        private void Start()
        {
            fx.gameObject.SetActive(false);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = other.gameObject.GetComponentInParent<Actor>();
                if (actor != null) actor.SpeedDown();
                gameObject.SetActive(false);
                fx.gameObject.SetActive(true);
            }
        }
    }
}