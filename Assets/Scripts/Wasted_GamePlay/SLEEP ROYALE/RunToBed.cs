using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ChaosHouse.AISettings;
using DG.Tweening;

namespace ChaosHouse
{
    public class RunToBed : SleepRoyaleBotLogic<RunToBedSettings>
    {
        private RunToBedSettings _bedSettings;
        private NavMeshPath _navMeshPath;
        private Queue<Vector3> _allPathPoints = new Queue<Vector3>();
        private bool _isSleep;
        private bool _isFalling;        
        private Vector3 _currentTarget;
        private float _currentSpeed;
        private Vector3 _currentDirection;

        public RunToBed(RunToBedSettings bedSettings)
        {
            _bedSettings = bedSettings;
            Init();
        }
        protected override void Init()
        {

            SetPath();
            
            EventManager.Subscribe(EEventsName.LevelComplite, (object o) => { Stop(); SetBalance(false); }); ;
            EventManager.Subscribe(EEventsName.LevelLost, (object o) => Sleep());
            SpeedUp();
        }

        private Vector3 GetTargetPoint()
        {
            var r = Random.Range(0, 100);
            if (r <= _bedSettings.ChanceGoTiFinish)
                return _bedSettings.BedPos;
            else
            {
                return GetRandomPoint(_bedSettings.Transform.position, 20f);
            }
        }

        private void SetBalance(bool state)
        {
            if (state) _bedSettings.AprController.balanceHeight = 1.7f;
            else _bedSettings.AprController.balanceHeight = 0f;
            _bedSettings.AprController.SetBalance(state);

        }

        public override void SetPath()
        {
            _navMeshPath = new NavMeshPath();
            _allPathPoints = new Queue<Vector3>();
            NavMesh.CalculatePath(_bedSettings.Transform.position, GetTargetPoint(), NavMesh.AllAreas, _navMeshPath);
            for (int i = 0; i < _navMeshPath.corners.Length; i++)
            {
                _allPathPoints.Enqueue(_navMeshPath.corners[i] + new Vector3(0, 1, 0));
            }
            _bedSettings.NavMeshAgent.enabled = false;
        }

        public override void Update()
        {
            if (!LevelController.Instance.InGame) return;
            if (_isSleep) return;

            Move();

            if (_isFalling)
            {
                _bedSettings.AprController.balanceHeight = Mathf.Lerp(_bedSettings.AprController.balanceHeight, 2f, Time.deltaTime);
                if (_bedSettings.AprController.balanceHeight > 1.9f) _isFalling = false;
            }
        }
        public override void Move()
        {
            if (_allPathPoints.Count > 0)
            {
                _currentTarget = _allPathPoints.Peek() - _bedSettings.AprController.Root.transform.position;

                if (Vector3.Distance(_bedSettings.AprController.Root.transform.position, _allPathPoints.Peek()) < 0.5f)
                    if (_allPathPoints.Count > 0)
                    {
                        _allPathPoints.Dequeue();
                    }
            }
            else
            {
                SetPath();
            }

            _bedSettings.AprController.PlayerMovement(true, Vector2.up, Time.deltaTime * _currentSpeed);
            _bedSettings.AprController.PlayerRotation(_currentTarget.normalized);

        }
        public override void Sleep()
        {
            if (_isSleep) return;
         
            _isSleep = true;
            Stop();
            _bedSettings.AprController.SetBalance(false);
            _bedSettings.AprController.DeactivateRagdoll();
            _bedSettings.AprController.balanceHeight = 0;
        }

        public override void Stop()
        {
            if (_bedSettings.NavMeshAgent == null) return;
            _bedSettings.AprController.PlayerMovement(false, _currentDirection, 0);
            _bedSettings.NavMeshAgent.enabled = false;
        }

        public override void OnTriggerEnter()
        {
            
        }

        public override void SpeedUp()
        {
            if (_currentSpeed != 0f) return;
            DOTween.To(() => _currentSpeed, x => _currentSpeed = x, _bedSettings.MovementSpeed, 2f);
        }

        public override void SpeedDown()
        {
            DOTween.To(() => _currentSpeed, x => _currentSpeed = x, 0f, 2f);
        }

        public override void Disable()
        {
            EventManager.Unsubscribe(EEventsName.LevelComplite, (object o) => { Stop(); SetBalance(false); }); ;
            EventManager.Unsubscribe(EEventsName.LevelLost, (object o) => Sleep());
        }

        public override void Falling()
        {
            if (!LevelController.Instance.InGame) return;
            if (_isFalling) return;
            var r = Random.Range(0, 100);
            if (r <= _bedSettings.ChanceTofall)
            {
                _isFalling = true;
                SetBalance(false);
                ActionWithDelayProvider.Instance.InvokeAction(DelayResetBalance, 2f);             
            }
        }

        private void DelayResetBalance()
        {
            _bedSettings.AprController.balanceHeight = 2f;
            _isFalling = false;
            SetBalance(true);

        }
    }
}
