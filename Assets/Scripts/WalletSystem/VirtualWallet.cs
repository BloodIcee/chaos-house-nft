using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse { 
    public static class VirtualWallet
    {
        private static string _prefsName = "VirtualWalletData";

        private static VirtualWalletData _virtualWalletData;

        public static VirtualWalletData WalletData { 
            get {
                if (_virtualWalletData == null)
                {
                    _virtualWalletData = Load();
                }
                return _virtualWalletData;
            }
        }

        private static VirtualWalletData Load()
        {
            if (PlayerPrefs.HasKey(_prefsName))
            {
                var savedStr = PlayerPrefs.GetString(_prefsName);
                VirtualWalletData virtualWalletData = JsonUtility.FromJson<VirtualWalletData>(savedStr);
                return virtualWalletData;
            }
            else
            {
                return new VirtualWalletData();
            }
        }
    }
}



