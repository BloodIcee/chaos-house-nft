using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public abstract class Player : Actor
    {

        public void SetName(string newName)
        {
            Name = newName;
        }

    }
}
