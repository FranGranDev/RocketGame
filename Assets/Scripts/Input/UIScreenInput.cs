using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Inputs
{
    public class UIScreenInput : MonoBehaviour, IScreenInput, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnTap;
        public event Action OnRelease;
        public event Action<Vector2> OnDrag;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnTap?.Invoke();
        }
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            OnRelease?.Invoke();
        }
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag?.Invoke(eventData.delta);
        }
    }
}