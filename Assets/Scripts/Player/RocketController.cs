using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Inputs;


namespace Game.Player
{
    [RequireComponent(typeof(IDrivable))]
    public class RocketController : MonoBehaviour
    {
        [SerializeField, Range(0.5f, 3f)] private float sensivity;

        [Inject]
        private IScreenInput screenInput;

        private IDrivable drivable;

        private void Awake()
        {
            drivable = GetComponent<IDrivable>();
        }
        private void OnEnable()
        {
            screenInput.OnDrag += OnDrag;
            screenInput.OnRelease += OnRelease;
            screenInput.OnTap += OnTap;
        }
        private void OnDisable()
        {
            screenInput.OnDrag -= OnDrag;
            screenInput.OnRelease -= OnRelease;
            screenInput.OnTap -= OnTap;
        }


        private void OnTap()
        {
            drivable.EngineOn();
        }
        private void OnRelease()
        {
            drivable.EngineOff();
        }
        private void OnDrag(Vector2 point)
        {
            drivable.Move(point * sensivity);
        }
    }
}
