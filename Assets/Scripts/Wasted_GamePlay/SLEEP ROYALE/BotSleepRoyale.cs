using ChaosHouse.AISettings;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ChaosHouse
{
    public class BotSleepRoyale : Bot
    {
        [SerializeField] float movementSpeed = 1f;
        [SerializeField] float rotationSpeed = 1f;
        [SerializeField] [Range(0, 100)] int chanceGoToFinish;
        [SerializeField] [Range(0, 100)] int chanceFall;

        private NavMeshAgent navMeshAgent;    

        private AILogic aiLogic;
        private void Start()
        {
            InitSettings();

            navMeshAgent = GetComponent<NavMeshAgent>();

            SetGameModeLogic(LevelController.Instance.GameMode);
            

        }

        private void SetGameModeLogic(EGameMode gameMode)
        {
            switch (gameMode)
            {
                case EGameMode.SleepRoyale:
                    var bedpos = FindObjectOfType<LevelModeController>().CurrentTarget.position;
                    RunToBedSettings runToBedSetting = new RunToBedSettings(APR_Controller, navMeshAgent, transform, movementSpeed, chanceGoToFinish, chanceFall, bedpos);
                    aiLogic = new RunToBed(runToBedSetting);
                    break;
                case EGameMode.RunToFindItem:
                    var itemSpawner = FindObjectOfType<ItemSpawner>();
                    RunToFindItemSettings runToFindItem = new RunToFindItemSettings(APR_Controller, navMeshAgent, transform, movementSpeed, chanceGoToFinish, chanceFall, itemSpawner);
                    aiLogic = new RunToFindItem(runToFindItem);
                    break;
                default:                    
                    runToBedSetting = new RunToBedSettings(APR_Controller, navMeshAgent, transform, movementSpeed, chanceGoToFinish, chanceFall, FindObjectOfType<LevelModeController>().CurrentTarget.position);
                    aiLogic = new RunToBed(runToBedSetting);
                    break;
            }
        }

        private void Update()
        {
            aiLogic.Update();
        }

        public override void Falling()
        {
            aiLogic.Falling();

        }

        public void SpeedUp()
        {
            aiLogic.SpeedUp();

        }

        public override void SpeedDown()
        {
            aiLogic.SpeedDown(); 
        }

        public override void Stop()
        {
            aiLogic.Stop();
        }


        public override void Sleep()
        {
            if (aiLogic.IsSleep) return;
            base.Sleep();

            aiLogic.Sleep();
        }


        public override void InitSettings()
        {
            var config = LevelController.Instance.SleepRoyaleConfig;

            var r = Random.Range(0, 0.2f);
            movementSpeed *= 0.8f + r;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ItemToFind>(out ItemToFind itemToFind))
            {
                TakeItem(itemToFind);
            }
        }

        private void OnDisable()
        {
            aiLogic?.Disable();
        }
  
    }
}
