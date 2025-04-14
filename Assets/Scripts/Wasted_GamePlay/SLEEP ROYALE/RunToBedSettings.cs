using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ChaosHouse.AISettings
{
    public class RunToBedSettings: iBotSetting
    {
        private APRController _aprController;
        private NavMeshAgent _navMeshAgent;       
        private Transform _transform;
        private float _movementSpeed;
        private float _chanceGoToFinish;
        private float _chanceTofall;
        private Vector3 _bedPos;
        public APRController AprController => _aprController;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public Transform Transform  => _transform;
        public float MovementSpeed =>_movementSpeed;
        public float ChanceGoTiFinish => _chanceGoToFinish;
        public float ChanceTofall => _chanceTofall;
        public Vector3 BedPos => _bedPos;

        public RunToBedSettings(APRController aprController, NavMeshAgent navMeshAgent, Transform transform, float movementSpeed, float chanceGoToFinish, float chanceTofall, Vector3 bedPos)
        {
            _aprController = aprController;
            _navMeshAgent = navMeshAgent;            
            _transform = transform;
            _movementSpeed = movementSpeed;
            _chanceGoToFinish = chanceGoToFinish;
            _chanceTofall = chanceTofall;
            _bedPos = bedPos;
        }
    }
}
