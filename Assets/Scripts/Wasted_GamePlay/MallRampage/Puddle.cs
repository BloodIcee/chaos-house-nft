using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class Puddle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = other.GetComponentInParent<Actor>();
                actor.Slide();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = other.GetComponentInParent<Actor>();
                actor.ResetHeightBalance();
            }
        }
    }
}