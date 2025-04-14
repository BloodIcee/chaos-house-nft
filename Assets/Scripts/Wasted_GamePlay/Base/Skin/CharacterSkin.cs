using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterSkin : MonoBehaviour
{
    [SerializeField] ESkin defaultSkin;

    private List<Skin> skins = new List<Skin>();
    public Skin currentSkin { get; private set; }
    public List<Skin> Skins { get { if (skins.Count == 0) InitSkins(); return skins;} }

    private void Awake()
    {
        InitSkins();        
    }
    private void InitSkins() {
        foreach (Transform child in transform)
        {
            var skin = child.GetComponent<Skin>();
            if (skin != null)
            {
                skins.Add(skin);
                if (skin.SkinType == defaultSkin)
                {
                    skin.gameObject.SetActive(true);
                    currentSkin = skin;
                }
                else skin.gameObject.SetActive(false);
            }
        }        
    }

    public void SetSkin(ESkin eSkin) {

        if (skins.Count == 0) InitSkins();
        var newSkin = skins.Find(x=> x.SkinType == eSkin);
        if (newSkin == null) return;
        currentSkin.gameObject.SetActive(false);
        newSkin.gameObject.SetActive(true);
        currentSkin = newSkin;
    }

    public void SetNextSkin() {

        var currentIndex = skins.FindIndex(x=> x == currentSkin);
        currentIndex++;
        if ((currentIndex) < skins.Count) SetSkin(skins[currentIndex].SkinType);
        else SetSkin(skins[0].SkinType);
    }

    public void SetPreviousSkin()
    {
        var currentIndex = skins.FindIndex(x => x == currentSkin);
        currentIndex--;
        if ((currentIndex) >= 0) SetSkin(skins[currentIndex].SkinType);
        else SetSkin(skins[skins.Count-1].SkinType);
    }

    public void SetRandomSkin(ESkin exceptionSkin) {
        if (skins.Count == 0) InitSkins();
        var availableSkins = skins.Where(x => x.SkinType != exceptionSkin).ToList();
        int r = Random.Range(0, availableSkins.Count);
        SetSkin(availableSkins[r].SkinType);
    }

    public Skin GetRandomSkin(List<Skin> skinList)
    {
        if (skins.Count == 0) InitSkins();
        int r = Random.Range(0, skinList.Count);
        return skinList[r];
    }
}
