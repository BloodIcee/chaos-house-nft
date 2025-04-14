using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class BotCollisionDetector : MonoBehaviour
    {
        Bot bot;

        private void Start()
        {
            bot = GetComponentInParent<Bot>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {

                if (!bot.collisionObj.Contains(collision.gameObject))
                {
                    bot.collisionObj.Add(collision.gameObject);
                    bot.Falling();
                    StartCoroutine(RemoveCollisionObj(collision.gameObject));
                }
            }
        }

        private IEnumerator RemoveCollisionObj(GameObject obj)
        {
            yield return new WaitForSeconds(2f);
            if (bot.collisionObj.Contains(obj)) bot.collisionObj.Remove(obj);
        }
    }
}