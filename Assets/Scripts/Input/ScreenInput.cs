using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Inputs
{
    public class ScreenInput : MonoBehaviour, IScreenInput
    {
        [Header("Settings")]
        [SerializeField] private LayerMask mask;


        private bool touched;
        private Vector2 lastPoint;


        public event Action OnTap;
        public event Action OnRelease;
        public event Action<Vector2> OnDrag;


        private void MouseInput()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && !touched)
            {
                Tap();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && touched)
            {
                Release();
            }
            if(Input.GetKey(KeyCode.Mouse0))
            {
                Move(Input.mousePosition);
            }
        }
        private void TapInput()
        {
            if (Input.touchCount > 0 && !touched)
            {
                Tap();
            }
            if (Input.touchCount == 0 && touched)
            {
                Release();
            }
            if (touched)
            {
                Move(Input.GetTouch(0).position);
            }
        }

        private void Tap()
        {
            touched = true;

            OnTap?.Invoke();
        }
        private void Release()
        {
            touched = false;
            lastPoint = default;

            OnRelease?.Invoke();
        }
        private void Move(Vector2 screenPoint)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);

            if(Physics.Raycast(ray, out RaycastHit hit, 1000, mask))
            {
                Vector2 point = hit.point;
                Vector2 delta = (point - lastPoint) / Time.deltaTime;

                if (lastPoint == default)
                {
                    delta = Vector2.zero;
                }
                lastPoint = point;

                OnDrag?.Invoke(delta);
            }
        }


        private void Update()
        {
#if UNITY_EDITOR
            MouseInput();
#else
            TapInput();
#endif
        }
        private void Start()
        {
            Vector3 position = transform.position;
            position.z = 0;

            transform.position = position;
        }
    }
}