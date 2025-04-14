using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [RequireComponent(typeof(APRController))]
    public class PlayerSleepRoyale : Player
    {
        [field: SerializeField] public CharacterView characterView { get; private set; }
        [SerializeField] private ActorName _actorNameUI;
        [SerializeField] private TriggerHelper _triggerHelper;

        private PlayerJoystick _playerJoystick;
        private APRController _aprController;

        [SerializeField] private float startSpeed = 1;
        [SerializeField] private float rotationSpeed = 10;
        private float targetSpeed;
        private Vector3 moveDirection;
        private Vector3 rotateDirection;
        private bool isSleep;

        public APRController APRController => _aprController;
        public bool InStairs { get; set; }

        private void OnEnable()
        {
            _actorNameUI.SetName(SaveManager.SavedData.PlayerName);

            _playerJoystick = FindObjectOfType<PlayerJoystick>();
            _aprController = GetComponent<APRController>();            

            _playerJoystick.onDown += _playerJoystick_onDown;
            _playerJoystick.onUp += _playerJoystick_onUp;
            _playerJoystick.onDrag += _playerJoystick_onDrag;

            _triggerHelper.OnTiggerEnterAction = OnTriggerEnterAction;
        }
        private void OnDisable()
        {
            _playerJoystick.onDown -= _playerJoystick_onDown;
            _playerJoystick.onUp -= _playerJoystick_onUp;
            _playerJoystick.onDrag -= _playerJoystick_onDrag;
        }

        private void _playerJoystick_onDrag(Vector2 obj)
        {
            moveDirection = new Vector3(obj.x, 0, obj.y);
            rotateDirection = moveDirection;
            targetSpeed = startSpeed;
        }

     
        private void _playerJoystick_onDown(Vector2 obj)
        {
            targetSpeed = 0f;
        }

        private void _playerJoystick_onUp(Vector2 obj)
        {
            targetSpeed = 0f;
            moveDirection = Vector3.zero;
        }

        private void HandleLevelLost(object o)
        {
            Stop();            
        }

        public override void Stop()
        {
            _aprController.balanceHeight = 0f;
        }

        private void Update()
        {
            if (!isSleep)
            {                
                _aprController.PlayerMovement(targetSpeed);
                _aprController.PlayerRotation(rotateDirection, rotationSpeed);
                _aprController.SetBalance(true);
            }
        }

        public override void Sleep()
        {
            if (isSleep) return;
            base.Sleep();
            isSleep = true;
            _aprController.PlayerMovement(0);
            _aprController.balanceHeight = 0f;
            EventManager.OnEvent(EEventsName.LevelComplite, this);

        }

        public override void InitSettings()
        {
            var config = LevelController.Instance.SleepRoyaleConfig;
            targetSpeed = config.MaxPlayerMovementSpeed;
            rotationSpeed = config.PlayerRotationSpeed;
        }

        private void OnTriggerEnterAction(Collider other)
        {
            if (other.TryGetComponent<ItemToFind>(out ItemToFind itemToFind))
            {
                TakeItem(itemToFind);
            }
        }
    }
}
