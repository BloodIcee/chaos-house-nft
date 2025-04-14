using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class BotSurviveMode : Bot
    {
        [SerializeField] Transform[] pathPoints;
        [SerializeField] CharacterSkinManager skin;

        private LevelController lc;
        private float movementSpeed;
        private float currentSpeed;
        private float balanceHeight;
        private Vector3 currentTarget;
        private Queue<Vector3> targets = new Queue<Vector3>();

        public bool isReceivedDamage { get; private set; }

        private void Awake()
        {
            base.Awake();
            lc = LevelController.Instance;
        }
        private void Start()
        {

            InitSettings();
            InitQueueTargets();

            EventManager.Subscribe(EEventsName.LevelComplite, (object o) => APR_Controller.balanceHeight = 0f);
            EventManager.Subscribe(EEventsName.LevelLost, (object o) => APR_Controller.balanceHeight = 0f);
            EventManager.Subscribe(EEventsName.LevelStart, (object o) => isMoved = true);
        }
        private void InitQueueTargets()
        {
            for (int i = 0; i < pathPoints.Length; i++)
            {
                targets.Enqueue(pathPoints[i].position);
            }

        }
        private void Update()
        {
            if (!lc.InGame) return;
            if (isMoved) Move();
        }

        private void Move()
        {
            SpeedUp();
            if (targets.Count > 0)
            {
                currentTarget = targets.Peek() - APR_Controller.Root.transform.position;

                if (Vector3.Distance(APR_Controller.Root.transform.position, targets.Peek()) < 2f)
                    if (targets.Count > 0)
                    {
                        targets.Dequeue();
                    }
            }

            APR_Controller.PlayerMovement(true, Vector2.up, currentSpeed);
            APR_Controller.PlayerRotation(currentTarget.normalized);
        }
        private void SpeedUp()
        {
            if (currentSpeed != 0f) return;
            DOTween.To(() => currentSpeed, x => currentSpeed = x, movementSpeed, LevelController.Instance.SurviveConfig.DurationAcceleration);
        }

        public override void SetMoveState(bool state)
        {
            base.SetMoveState(state);
            isMoved = state;
        }
        public override void SpeedDown()
        {
            DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, LevelController.Instance.SurviveConfig.BrakingSpeed);
        }

        public override void InitSettings()
        {
            var r = Random.Range(0, 0.2f);
            movementSpeed = lc.SurviveConfig.MaxBotSpeed * (1 + r);

            balanceHeight = APR_Controller.balanceHeight;
        }

        public override void Sleep()
        {
            APR_Controller.balanceHeight = 0;
        }

        public override void Slide()
        {
            base.Slide();
            APR_Controller.balanceHeight = 0;
            var rb = APR_Controller.Root.GetComponent<Rigidbody>();
            rb.AddForce(APR_Controller.transform.forward * 20f, ForceMode.Impulse);
            rb.AddTorque(Vector3.up * 120f, ForceMode.Impulse);
        }

        public override void ResetHeightBalance()
        {
            base.ResetHeightBalance();
            if (APR_Controller.balanceHeight > 0) return;
            StartCoroutine(ResetBalance());
        }

        private IEnumerator ResetBalance()
        {
            yield return new WaitForSeconds(1.2f);
            APR_Controller.balanceHeight = balanceHeight * 2f;
            APR_Controller.SetBalance(true);
        }

        public void Damage()
        {
            if (isReceivedDamage) return;
            isReceivedDamage = true;
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1f);
            isReceivedDamage = false;
        }

        private void Hit(Vector3 targetPos)
        {
            if (transform.position.x > targetPos.x)
                APR_Controller.PunchLeft(targetPos);
            else APR_Controller.PunchRight(targetPos);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = other.GetComponent<Actor>();
                if (actor != null) Hit(actor.Head.position);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player_1"))
            {
                var actor = collision.gameObject.GetComponent<Actor>();
                var direction = collision.gameObject.transform.position - transform.position;
                if (actor != null) actor.GetComponent<Rigidbody>().AddForce(direction * 10f);
            }
        }

        public override void TakeDamage(float damage, EDirection pushDirection, float pushForce)
        {
            if (!isMoved) return;
            Stop();

            if (pushDirection == EDirection.Back) APR_Controller.Root.GetComponent<Rigidbody>().AddForce((-transform.forward + Vector3.up) * pushForce, ForceMode.Impulse);
            else if (pushDirection == EDirection.Left) APR_Controller.Root.GetComponent<Rigidbody>().AddForce((-transform.right + Vector3.up) * pushForce, ForceMode.Impulse);
            else if (pushDirection == EDirection.Right) APR_Controller.Root.GetComponent<Rigidbody>().AddForce((transform.right + Vector3.up) * pushForce, ForceMode.Impulse);
            StartCoroutine(ResetMoving());
        }
        private IEnumerator ResetMoving()
        {
            yield return new WaitForSeconds(0.5f);
            isMoved = true;
        }
        public override void Stop()
        {
            isMoved = false;
            SpeedDown();
            APR_Controller.PlayerMovement(false, Vector2.zero, 0);
        }

    }
}
