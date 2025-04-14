using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace ChaosHouse
{
    public class ScrollButton : MonoBehaviour
    {
        [SerializeField] EDirection eDirection;
        [SerializeField] UnityEvent actionsShowPrice;
        [SerializeField] UnityEvent actionShowMode;
        [SerializeField] Animator playerAnimator;

        private CharacterSkinManager characterSkin;
        private Animator animator;
        private ButtonBuy _buttonBuy;

        public void Click()
        {
            animator.Rebind();
            animator.SetTrigger("Click");
            playerAnimator.SetTrigger("ChangeSkin");
            if (eDirection == EDirection.Right) characterSkin.SetNextSkin();
            else if (eDirection == EDirection.Left) characterSkin.SetPreviousSkin();

            if (!VirtualWallet.WalletData.ContainsCharacterData(characterSkin.CurrentCharacterData))
            {
                actionsShowPrice?.Invoke();
                if(_buttonBuy==null) _buttonBuy = FindObjectOfType<ButtonBuy>();
                _buttonBuy.UpdatePrice();
            }
            else actionShowMode?.Invoke();
        }

        private void Start()
        {
            characterSkin = FindObjectOfType<CharacterSkinManager>();
            animator = GetComponent<Animator>();
        }


    }
}
