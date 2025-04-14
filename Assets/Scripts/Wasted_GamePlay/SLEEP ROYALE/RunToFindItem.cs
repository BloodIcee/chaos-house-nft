using ChaosHouse.AISettings;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
namespace ChaosHouse 
{
    public class RunToFindItem : SleepRoyaleBotLogic<RunToFindItemSettings>
    {
        private RunToFindItemSettings _startData;
        private NavMeshPath _navMeshPath;
        private Queue<Vector3> _allPathPoints = new Queue<Vector3>();
        private bool _isSleep;
        private bool _isFalling;
        private Vector3 _currentTarget;
        private float _currentSpeed;
        private Vector3 _currentDirection;

        public RunToFindItem (RunToFindItemSettings startData)
        {
            _startData = startData;
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
            if (r <= _startData.ChanceGoTiFinish)
                return _startData.ItemSpawner.GetRandomItemPosition();
            else
            {
                return GetRandomPoint(_startData.Transform.position, 20f);
            }
        }

        private void SetBalance(bool state)
        {
            if (state) _startData.AprController.balanceHeight = 1.7f;
            else _startData.AprController.balanceHeight = 0f;
            _startData.AprController.SetBalance(state);

        }

        public override void SetPath()
        {
            _navMeshPath = new NavMeshPath();
            _allPathPoints = new Queue<Vector3>();
            NavMesh.CalculatePath(_startData.Transform.position, GetTargetPoint(), NavMesh.AllAreas, _navMeshPath);
            for (int i = 0; i < _navMeshPath.corners.Length; i++)
            {
                _allPathPoints.Enqueue(_navMeshPath.corners[i] + new Vector3(0, 1, 0));
            }
            _startData.NavMeshAgent.enabled = false;
        }

        public override void Update()
        {
            if (!LevelController.Instance.InGame) return;
            if (_isSleep) return;

            Move();

            if (_isFalling)
            {
                _startData.AprController.balanceHeight = Mathf.Lerp(_startData.AprController.balanceHeight, 2f, Time.deltaTime);
                if (_startData.AprController.balanceHeight > 1.9f) _isFalling = false;
            }
        }
        public override void Move()
        {
            if (_allPathPoints.Count > 0)
            {
                _currentTarget = _allPathPoints.Peek() - _startData.AprController.Root.transform.position;

                if (Vector3.Distance(_startData.AprController.Root.transform.position, _allPathPoints.Peek()) < 0.5f)
                    if (_allPathPoints.Count > 0)
                    {
                        _allPathPoints.Dequeue();
                    }
            }
            else
            {
                SetPath();
            }

            _startData.AprController.PlayerMovement(true, Vector2.up, Time.deltaTime * _currentSpeed);
            _startData.AprController.PlayerRotation(_currentTarget.normalized);

        }
        public override void Sleep()
        {
            if (_isSleep) return;

            _isSleep = true;
            Stop();
            _startData.AprController.SetBalance(false);
            _startData.AprController.DeactivateRagdoll();
            _startData.AprController.balanceHeight = 0;
        }

        public override void Stop()
        {
            if (_startData.NavMeshAgent == null) return;
            _startData.AprController.PlayerMovement(false, _currentDirection, 0);
            _startData.NavMeshAgent.enabled = false;
        }

        public override void OnTriggerEnter()
        {

        }

        public override void SpeedUp()
        {
            if (_currentSpeed != 0f) return;
            DOTween.To(() => _currentSpeed, x => _currentSpeed = x, _startData.MovementSpeed, 2f);
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
            if (r <= _startData.ChanceTofall)
            {
                _isFalling = true;
                SetBalance(false);
                ActionWithDelayProvider.Instance.InvokeAction(DelayResetBalance, 2f);
            }
        }

        private void DelayResetBalance()
        {
            _startData.AprController.balanceHeight = 2f;
            _isFalling = false;
            SetBalance(true);

        }
    }
}
