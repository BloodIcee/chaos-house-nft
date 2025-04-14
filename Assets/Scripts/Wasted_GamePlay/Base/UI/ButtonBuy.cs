using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ChaosHouse
{
    public class ButtonBuy : ButtonBase
    {
        [SerializeField] private Text priceText;
        [SerializeField] private UnityEvent action;

        private CharacterSkinManager characterSkin;

        private PlayerInUI player;
        private SkinConfig skinConfig;
        private int price;
        private LevelController lc;

        protected override void Awake()
        {
            characterSkin = FindObjectOfType<CharacterSkinManager>();
        }
        private void Start()
        {
            lc = LevelController.Instance;
            skinConfig = lc.SkinConfig;
            UpdatePrice();
        }

        public void UpdatePrice()
        {
            price = 100;
            priceText.text = TextFormater.CoinFormat("", price);
            //player = FindObjectOfType<PlayerInUI>();
            //price = skinConfig.GetSkinPrice(player.characterSkin.currentSkin.SkinType);
            //priceText.text = TextFormater.CoinFormat("", price);
            if (price == 0 || VirtualWallet.WalletData.ContainsCharacterData(characterSkin.CurrentCharacterData))
               action?.Invoke();
        }

        protected override void ButtonFunc()
        {
            if (VirtualWallet.WalletData.CoinValue >= price)
            {
                VirtualWallet.WalletData.TakeCoin((uint)price);
                VirtualWallet.WalletData.AddNewCharacterData(characterSkin.CurrentCharacterData);                               
                SaveManager.SavedData.ChangeCurrentCharacterData(characterSkin.IndexCurrentCharactedData);

                action?.Invoke();
            }
        }
    }
}