using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class FinishPoint : MonoBehaviour
    {
        [SerializeField] float jumpForce;
        private List<Actor> actors = new List<Actor>();

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = collision.gameObject.GetComponentInParent<Actor>();
                if (!actors.Contains(actor))
                {
                    actors.Add(actor);
                    actor.PlaceNumber = actors.Count;
                    actor.Sleep();
                }

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = other.gameObject.GetComponentInParent<Actor>();
                actor.Jump(transform.position + Vector3.down, jumpForce);
            }
        }
    }
}
