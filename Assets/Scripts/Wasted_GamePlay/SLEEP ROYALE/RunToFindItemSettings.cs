using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace ChaosHouse.AISettings
{
    public class RunToFindItemSettings : iBotSetting
    {
        private APRController _aprController;
        private NavMeshAgent _navMeshAgent;
        private Transform _transform;
        private float _movementSpeed;
        private float _chanceGoToFinish;
        private float _chanceTofall;
        private ItemSpawner _itemSpawner;
        public APRController AprController => _aprController;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public Transform Transform => _transform;
        public float MovementSpeed => _movementSpeed;
        public float ChanceGoTiFinish => _chanceGoToFinish;
        public float ChanceTofall => _chanceTofall;
        public ItemSpawner ItemSpawner => _itemSpawner;

        public RunToFindItemSettings(APRController aprController, NavMeshAgent navMeshAgent, Transform transform, float movementSpeed, float chanceGoToFinish, float chanceTofall, ItemSpawner itemSpawner)
        {
            _aprController = aprController;
            _navMeshAgent = navMeshAgent;
            _transform = transform;
            _movementSpeed = movementSpeed;
            _chanceGoToFinish = chanceGoToFinish;
            _chanceTofall = chanceTofall;
            _itemSpawner = itemSpawner;
        }
    }
}
