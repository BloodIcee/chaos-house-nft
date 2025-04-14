using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ChaosHouse
{
    public class TapticCollisionChecker : MonoBehaviour
    {
        private float delay = 2f;
        private bool inGame = true;
        private Player player;
        private void Start()
        {
            inGame = true;
            player = FindObjectOfType<Player>();
            EventManager.Subscribe(EEventsName.LevelComplite, OnLevelComplete);
        }

        private void OnLevelComplete(object o)
        {

            inGame = false;
            if (player.PlaceNumber == 1) Taptic.Success();
            else Taptic.Failure();
        }
        private void OnCollisionEnter(Collision collision)
        {
            //if (!inGame) return;
            //if (player.collisionObj.Contains(collision.gameObject)) return;


            //if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Furniture"))
            //{
            //    Taptic.Medium();
            //    player.collisionObj.Add(collision.gameObject);
            //    StartCoroutine(RemoveElementInList(collision.gameObject));
            //}

            //if (collision.gameObject.GetComponentInParent<Bot>() != null)
            //{
            //    Taptic.Light();
            //    player.collisionObj.Add(collision.gameObject);
            //    StartCoroutine(RemoveElementInList(collision.gameObject));
            //}

        }

        //private IEnumerator RemoveElementInList(GameObject o)
        //{
        //    yield return new WaitForSeconds(delay);
        //    if (player.collisionObj.Contains(o)) player.collisionObj.Remove(o);
        //}
    }
}
