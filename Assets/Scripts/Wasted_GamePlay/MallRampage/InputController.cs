using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ChaosHouse
{
    public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private float sensitive;

        private Vector2 currentSwipe;
        private PlayerSurviveMode player;
        private void Start()
        {
            player = FindObjectOfType<PlayerSurviveMode>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            player.Run();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            player.Stop();
            player.UpdateDirection(Vector2.zero);
        }

        public void OnDrag(PointerEventData eventData)
        {
            currentSwipe = eventData.delta * sensitive;
            player.UpdateDirection(currentSwipe);
        }

    }
}
