using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosHouse
{
    public class CoinCounter : MonoBehaviour
    {
        [SerializeField] Text coinText;

        private void Start()
        {
            var coin = LevelController.Instance.Coin;
            coinText.text = TextFormater.CoinFormat("", coin);
            EventManager.Subscribe(EEventsName.BuySkin, HandleBuySkin);
        }

        private void HandleBuySkin(object o)
        {
            var coin = LevelController.Instance.Coin;
            coinText.text = TextFormater.CoinFormat("", coin);
        }
    }
}