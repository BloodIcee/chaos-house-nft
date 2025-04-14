using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class DangerousObject : MonoBehaviour
    {
        [SerializeField] float damageSize;
        [SerializeField] EDirection pushDirection;
        [SerializeField] float pushForce = 100;


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = collision.gameObject.GetComponentInParent<Actor>();
                if (actor != null) actor.TakeDamage(damageSize, pushDirection, pushForce);
            }
        }
    }
}