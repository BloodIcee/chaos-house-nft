using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class TriggerHelper : MonoBehaviour
    {
        public Action<Collider> OnTiggerEnterAction;
        private void OnTriggerEnter(Collider other)
        {
            OnTiggerEnterAction?.Invoke(other);
        }
    }
}