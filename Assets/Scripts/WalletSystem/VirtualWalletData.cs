using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    [System.Serializable]
    public class VirtualWalletData
    {
       
        [SerializeField] private uint _coinValue = 0;
        [SerializeField] private List<CharacterData> _characterDatas = new List<CharacterData>();

        public uint CoinValue => _coinValue;
        public IReadOnlyList<CharacterData> CharacterDatas => _characterDatas;

        public bool ContainsCharacterData(CharacterData characterData)
        {
            return _characterDatas.Contains(characterData);
        }
        public void AddCoin(uint value)
        {
            _coinValue += value;
        }
        public void TakeCoin(uint value)
        {
            _coinValue -= value;
        }
        public void AddNewCharacterData(CharacterData characterData)
        {
            if(!_characterDatas.Contains(characterData)) _characterDatas.Add(characterData);
        }
    }
}
