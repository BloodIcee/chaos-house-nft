using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ChaosHouse
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollower : MonoBehaviour
    {

        //Player root
        private Transform aprRoot;
        private Transform startTarget;
        [Header("Follow Properties")]
        //Follow values
        public float distance = 10.0f;
        public float smoothness = 0.15f;
        public float maxDistance = 10f;
        public Vector3 AddOffset;
        public Vector3 OnBedLookOffset;
        [Header("Rotation")]
        [Range(-1f, 1f)]
        public float additiveRotate_Z;
        public float rotateSpeed;
        
        private Vector3 offset;
        private bool inGame;
        private void Start()
        {            
            offset = transform.position;
            aprRoot = FindObjectOfType<PlayerSleepRoyale>().APRController.Root.transform;

            if (LevelController.Instance.GameMode == EGameMode.SleepRoyale)
            {
                startTarget = FindObjectOfType<LevelModeController>().CurrentTarget;
                SetStartPos();
            }

            
            EventManager.Subscribe(EEventsName.LevelStart, HandleLevelStart);
        }

        private void SetStartPos()
        {
            transform.DOMove(OnBedLookOffset + startTarget.position + offset * distance / maxDistance, 1f);
            transform.DOLookAt(startTarget.position - transform.position, 1f);
            //var targetRotation = Quaternion.LookRotation(startTarget.position - transform.position);
            //transform.position = OnBedLookOffset + startTarget.position + offset * distance / maxDistance;
            //transform.rotation = targetRotation;
        }
        private void HandleLevelStart(object o)
        {
            inGame = true;
        }

        private void Update()
        {
            if (inGame)
            {
                var targetRotation = Quaternion.LookRotation(aprRoot.position - transform.position);
                targetRotation.z += additiveRotate_Z;
                transform.position = Vector3.Lerp(transform.position, AddOffset + aprRoot.position + offset * distance / maxDistance, Time.deltaTime * smoothness);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
        }
    }
}
