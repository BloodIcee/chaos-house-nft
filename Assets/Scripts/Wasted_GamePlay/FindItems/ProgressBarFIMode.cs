using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosHouse
{
    public class ProgressBarFIMode : MonoBehaviour
    {
        [SerializeField] private Image _progressImage;
        [SerializeField] private Text _progressText;

        private ItemSpawner _itemSpawner;
        private void OnEnable()
        {
            _itemSpawner = FindObjectOfType<ItemSpawner>();
            
        }

        private void Update()
        {
            _progressText.text = $"{_itemSpawner.CountItemInStart - _itemSpawner.CurrentCountItem}/{_itemSpawner.CountItemInStart}";
            float progressValue = 1 - (float)_itemSpawner.CurrentCountItem / (float)_itemSpawner.CountItemInStart;
            _progressImage.fillAmount = progressValue;
        }
    }
}
