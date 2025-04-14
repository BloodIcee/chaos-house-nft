using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosHouse
{
    public class HealthCanvas : MonoBehaviour
    {
        [SerializeField] private Image healthImage;
        private PlayerSurviveMode player;


        private void Start()
        {
            player = FindObjectOfType<PlayerSurviveMode>();
            healthImage.fillAmount = player.Health;

            EventManager.Subscribe(EEventsName.LevelComplite, HideHealth);
            EventManager.Subscribe(EEventsName.LevelLost, HideHealth);
        }
        private void HideHealth(object o)
        {

            gameObject.SetActive(false);
        }

        private void Update()
        {
            healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, player.Health, Time.deltaTime);
        }

    }
}