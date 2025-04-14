using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class Bed : MonoBehaviour
    {
        [SerializeField] float jumpforce;
        [SerializeField] GameObject[] activedTramplines;

        public bool IsActive { get; private set; }
        public bool Busy { get; private set; }
        private List<Actor> actors = new List<Actor>();

        public void Activeted()
        {
            IsActive = true;
            foreach (var obj in activedTramplines) obj.SetActive(true);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = collision.gameObject.GetComponentInParent<Actor>();
                if (!actors.Contains(actor))
                {
                    actors.Add(actor);
                    actor.Sleep();
                    actor.SetBalance(false);
                    actor.PlaceNumber = actors.Count;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = other.gameObject.GetComponentInParent<Actor>();
                if (!actors.Contains(actor))
                {
                    actor.Jump(transform.position + Vector3.up, jumpforce);                   
                }
            }
        }
    }
}