using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace ChaosHouse
{
    public class ShopingCart : MonoBehaviour
    {
        [SerializeField] Transform endTarget;
        [SerializeField] float forceJump;
        [SerializeField] Transform actorPos;

        private bool isFull;
        private bool isPush;
        private Rigidbody rb;
        private Actor _actor;
        private float moveDuration = 2f;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (isFull || !isPush) return;
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1") && collision.gameObject.GetComponentInParent<Player>() != null)
            {
                if (_actor == null) return;
                _actor.Stop();
                _actor.SetBalance(false);
                _actor.transform.parent = actorPos;
                _actor.transform.LookAt(actorPos);
                _actor.transform.position += Vector3.up;
                _actor.GetComponent<Rigidbody>().isKinematic = true;
                _actor.transform.DORotate(_actor.transform.eulerAngles + new Vector3(120, 0, 0), 0.5f);


                Move();
                isFull = true;

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isPush) return;
            if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                _actor = other.gameObject.GetComponentInParent<Player>();
                if (_actor == null) return;
                isPush = true;
            }
        }
        private IEnumerator ResetParent()
        {
            yield return new WaitForSeconds(moveDuration - 0.1f);
            _actor.transform.parent = null;
            _actor.SetMoveState(true);
            _actor.SetBalance(true);
            _actor.GetComponent<Rigidbody>().isKinematic = false;
        }

        private void Move()
        {
            var pos = endTarget.position;
            pos.y = transform.position.y;
            transform.DOMove(pos, moveDuration);
            StartCoroutine(ResetParent());
        }
    }
}