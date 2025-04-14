using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public abstract class Actor : MonoBehaviour
    {
        public int PlaceNumber { get; set; }
        public string Name { get; set; }
        protected APRController APR_Controller;
        public CharacterSkinManager characterSkin { get; private set; }
        public bool isMoved { get; set; }
        public Transform Head { get; private set; }
        public int NumberOfItemFound { get; private set; }
        public virtual void Sleep()
        {
            FXController.Instance.CreateFXInPoint(eFXType.sleep, APR_Controller.Head.transform.position);
        }
        public abstract void InitSettings();
        public virtual void Slide()
        {
            FXController.Instance.CreateFXInPoint(eFXType.puddle, APR_Controller.RightFoot.transform.position);
        }

        public void SetPlaceNumber(int number)
        {
            PlaceNumber = number;
        }

        public void TakeItem(ItemToFind itemToFind)
        {
            NumberOfItemFound++;
            itemToFind.Take();
        }

        public virtual void ResetHeightBalance() { }
        public virtual void Stop() { }
        public virtual void SpeedDown() { }
        public virtual void TakeDamage(float damage, EDirection pushDirection, float pushForce) { }
        public void Jump(Vector3 point, float force) { APR_Controller.JumpToPoint(transform.position - point, force); }
        public virtual void SetMoveState(bool state) { }
        public void SetBalance(bool state)
        {
            if (state) APR_Controller.balanceHeight = 1.7f;
            else APR_Controller.balanceHeight = 0f;
            APR_Controller.SetBalance(state);

        }

        public virtual void Awake()
        {
            characterSkin = GetComponentInParent<CharacterSkinManager>();
            APR_Controller = GetComponentInParent<APRController>();
            this.Head = APR_Controller.Head.transform;
        }
    }
}
