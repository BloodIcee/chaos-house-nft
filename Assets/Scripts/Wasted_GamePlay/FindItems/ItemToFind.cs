using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class ItemToFind : MonoBehaviour
    {
        public Action<ItemToFind> onFinded;

        public void Take()
        {
            onFinded?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
