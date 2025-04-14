using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChaosHouse.CharacterGenerator
{
    public interface IToolbarItem<out T> where T : CharacterPart
    {
        T CharacterPart { get; }
        string ItemName { get; }

        event Action onChanged;
        void Draw();


    }
}