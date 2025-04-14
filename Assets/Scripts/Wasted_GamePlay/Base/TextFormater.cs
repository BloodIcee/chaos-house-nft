using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextFormater
{
    private static Dictionary<float, string> keyValuePairs = new Dictionary<float, string>() {
        {Mathf.Pow(10,3), "k" },
        {Mathf.Pow(10,6), "m" },
        {Mathf.Pow(10,9), "b" },
    };
    public static string CoinFormat(string startStr, float coin)
    {
        string str = "";
        str = startStr + string.Format("{0:0.##}", coin);
        if (coin < Mathf.Pow(10, 3)) return str;

        foreach (var k in keyValuePairs) {      
            if (coin / k.Key > 0.1f) {
                str = startStr + GetStr(coin, k.Key) + k.Value;
            }
        }
         
        return str;
    }

    private static string GetStr(float coin, float max)
    {
        float cel = Mathf.RoundToInt(coin / max);
        cel += (coin - cel * max) / max;
        return string.Format("{0:0.##}", cel);
    }

}
