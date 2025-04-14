using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "CoinConfig", menuName = "Configs/CoinConfig")]
public class CoinConfig : ScriptableObject
{
    public List<CoinData> CoinDatas = new List<CoinData>();

    public int GetRewardCoin(int numerPos) {
        var coin = CoinDatas.Find(x=> x.PlaceNumber == numerPos);
        return coin.CoinSize;
    }
}

[System.Serializable]
public class CoinData
{
    public int PlaceNumber;
    public int CoinSize;

}
