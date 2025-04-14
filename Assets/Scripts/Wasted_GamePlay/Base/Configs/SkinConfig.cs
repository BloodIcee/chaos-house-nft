using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "SkinConfig", menuName = "Configs/SkinConfig")]
public class SkinConfig : ScriptableObject
{
    public List<SkinData> skinDatas;
   
    public ESkin GetFreeSkin()
    {
        var skin = skinDatas.Find(x=> x.Price ==0).skinType;
        return skin;
    }

    public int GetSkinPrice(ESkin skinType) {
        var price = skinDatas.Find(x => x.skinType == skinType).Price;
        return price;
    }
}

[System.Serializable]
public class SkinData {
    public ESkin skinType;
    public int Price;
}