using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ChaosHouse
{
    public class PlayerSurviveMode : Player
    {
        private float targetSpeed;
        private float currentSpeed;

        private bool IsLive = true;
        private Vector2 swipeOffset;
        private float balanceHeight;
        public float Health { get; private set; } = 1f;

        private void Start()
        {
            EventManager.Subscribe(EEventsName.LevelLost, HandleLevelLost);
            InitSettings();
        }

        private void HandleLevelLost(object o)
        {
            Stop();
            IsLive = false;
            APR_Controller.balanceHeight = 0f;
        }

        private void Update()
        {
            if (IsLive && isMoved)
            {
                SpeedUp();
                APR_Controller.PlayerMovement(isMoved, swipeOffset, currentSpeed);
            }
        }

        private void SpeedUp()
        {
            if (currentSpeed != 0f) return;
            DOTween.To(() => currentSpeed, x => currentSpeed = x, targetSpeed, LevelController.Instance.SurviveConfig.DurationAcceleration);
        }

        public override void SpeedDown()
        {
            DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, LevelController.Instance.SurviveConfig.BrakingSpeed);
        }


        public override void TakeDamage(float damage, EDirection pushDirection, float pushForce)
        {
            if (!isMoved) return;
            SpeedDown();

            if (pushDirection == EDirection.Back) APR_Controller.Root.GetComponent<Rigidbody>().AddForce((-transform.forward + Vector3.up) * pushForce, ForceMode.Impulse);
            else if (pushDirection == EDirection.Left) APR_Controller.Root.GetComponent<Rigidbody>().AddForce((-transform.right + Vector3.up) * pushForce, ForceMode.Impulse);
            else if (pushDirection == EDirection.Right) APR_Controller.Root.GetComponent<Rigidbody>().AddForce((transform.right + Vector3.up) * pushForce, ForceMode.Impulse);

        }

        public override void Slide()
        {
            base.Slide();
            APR_Controller.balanceHeight = 0;
            var rb = APR_Controller.Root.GetComponent<Rigidbody>();
            rb.AddForce(APR_Controller.transform.forward * 20f, ForceMode.Impulse);
            rb.AddTorque(Vector3.up * 120f, ForceMode.Impulse);
        }

        public void Run() => isMoved = true;
        public override void Stop()
        {
            isMoved = false;
            SpeedDown();
            APR_Controller.PlayerMovement(isMoved, swipeOffset, currentSpeed);
        }
        public void UpdateDirection(Vector2 swipeCoord)
        {
            swipeOffset = swipeCoord;
        }

        public override void Sleep()
        {
            base.Sleep();
            APR_Controller.balanceHeight = 0;
            APR_Controller.PlayerMovement(false, Vector2.zero, 0);
            EventManager.OnEvent(EEventsName.LevelComplite, this);
        }

        public override void InitSettings()
        {
            balanceHeight = APR_Controller.balanceHeight;
            targetSpeed = LevelController.Instance.SurviveConfig.MaxMoveSpeed;
        }

        public override void ResetHeightBalance()
        {
            base.ResetHeightBalance();
            APR_Controller.balanceHeight = balanceHeight;
            APR_Controller.SetBalance(true);
        }

        private void Hit(Vector3 targetPos)
        {
            if (APR_Controller.Root.transform.position.x > targetPos.x)
                APR_Controller.PunchLeft(targetPos);
            else APR_Controller.PunchRight(targetPos);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var bot = other.GetComponent<BotSurviveMode>();
                if (bot != null) Hit(bot.Head.position);
            }
        }

    }
}
