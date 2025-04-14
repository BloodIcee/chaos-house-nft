using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class Trampoline : MonoBehaviour
    {
        [SerializeField] float jumpForce;
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = collision.gameObject.GetComponentInParent<Actor>();
                if (actor != null)
                {
                    actor.Jump(-Vector3.up * jumpForce + collision.gameObject.transform.forward, jumpForce);
                    animator.SetTrigger("Jump");
                }
            }
        }

    }
}