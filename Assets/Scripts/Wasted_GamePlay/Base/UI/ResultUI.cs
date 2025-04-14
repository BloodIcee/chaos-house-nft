using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosHouse
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] Text positionText;
        [SerializeField] Text coinText;
        [SerializeField] Text suffixText;
        [SerializeField] CoinConfig coinConfig;
        [SerializeField] NoThanksButton rewardButton;
        [SerializeField] GameObject glow;
        private Player player;
        private List<Actor> actors = new List<Actor>();
        private void Start()
        {
            player = LevelController.Instance.player;

            if (LevelController.Instance.GameMode == EGameMode.RunToFindItem)
            {
                actors = FindObjectsOfType<Actor>().ToList();
                var sortedActors = actors.OrderByDescending(x => x.NumberOfItemFound);
                var playerIndex = sortedActors.ToList().IndexOf(player);
                player.SetPlaceNumber(playerIndex+1);
            }

            positionText.text = player.PlaceNumber.ToString();
            var coin = coinConfig.GetRewardCoin(player.PlaceNumber);
            coinText.text = TextFormater.CoinFormat("", coin);
            rewardButton.CoinSize = coin;
            if (player.PlaceNumber > 1) glow.SetActive(false);
            SetSuffix(player.PlaceNumber);
        }

        private void SetSuffix(int number)
        {
            switch (number)
            {
                case 1:
                    suffixText.text = "st";
                    break;
                case 2:
                    suffixText.text = "nd";
                    break;
                case 3:
                    suffixText.text = "rd";
                    break;
                case 4:
                    suffixText.text = "th";
                    break;
                case 5:
                    suffixText.text = "th";
                    break;
            }
        }
    }
}
