using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class NoThanksButton : ButtonBase
    {
        public int CoinSize { get; set; }
        protected override void ButtonFunc()
        {            
            VirtualWallet.WalletData.AddCoin((uint)CoinSize);
            EventManager.OnEvent(EEventsName.LoadMenu);
        }
    }
}