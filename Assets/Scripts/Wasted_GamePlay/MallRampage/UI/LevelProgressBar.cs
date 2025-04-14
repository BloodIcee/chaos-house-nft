using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosHouse
{
    public class LevelProgressBar : MonoBehaviour
    {
        [SerializeField] private Image progressFill;

        private APRController player;
        private FinishPoint finishPoint;
        private float startDistance;
        private float currentDistance;

        private void Start()
        {
            player = FindObjectOfType<APRController>();
            finishPoint = FindObjectOfType<FinishPoint>();
            progressFill.fillAmount = 0f;
            startDistance = Vector3.Distance(player.Root.transform.position, finishPoint.transform.position);
        }

        private void Update()
        {
            currentDistance = Vector3.Distance(player.Root.transform.position, finishPoint.transform.position);
            progressFill.fillAmount = 1f - currentDistance / startDistance;
        }

    }
}
