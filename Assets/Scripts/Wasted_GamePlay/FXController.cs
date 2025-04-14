using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFXType { sleep, puddle}
public class FXController : Singleton<FXController>
{
   [SerializeField] GameObject pfbSleepFX;
   [SerializeField] GameObject pfbPuddleFX;

    public void CreateFXInPoint(eFXType fXType, Vector3 pos) {
        var pfb = GetPfb(fXType);
        GameObject fx = Instantiate(pfb);
        fx.transform.position = pos;
    }

    private GameObject GetPfb(eFXType fXType) {
        switch (fXType) {
            case eFXType.puddle:
                return pfbPuddleFX;
                break;
            case eFXType.sleep:
                return pfbSleepFX;
                break;
            default: return pfbSleepFX;

        }

    } 
}
