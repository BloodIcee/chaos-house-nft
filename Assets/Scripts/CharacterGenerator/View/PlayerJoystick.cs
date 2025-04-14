using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace ChaosHouse
{
    public class PlayerJoystick : DynamicJoystick
    {
        public event Action<Vector2> onDown; 
        public event Action<Vector2> onUp; 
        public event Action<Vector2> onDrag; 

        protected override void Start()
        {
            base.Start();           
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            onDown?.Invoke(Direction);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            onUp?.Invoke(Direction);
        }

        protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            base.HandleInput(magnitude, normalised, radius, cam);
            onDrag?.Invoke(Direction);

        }
    }
}