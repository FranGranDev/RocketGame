using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Services;


namespace Game.Enviroment
{
    [RequireComponent(typeof(Collider))]
    public class LooseTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float minHeight;
        [Header("Links")]
        [SerializeField] private InfinityPipeCreator pipeCreator;

        [Inject]
        private IGameEvents gameEvents;

        private new Collider collider;
        private float topHeight = float.MinValue;


        public event System.Action OnPlayerEnter;


        private void Awake()
        {
            collider = GetComponent<Collider>();

            gameEvents.OnChanged += GameStateChanged;
        }

        private void GameStateChanged(GameStates obj)
        {
            if(obj == GameStates.Game)
            {
                topHeight = float.MinValue;
            }

            collider.enabled = obj == GameStates.Game;
        }
        private void UpdateTrigger()
        {
            topHeight = Mathf.Max(pipeCreator.CurrantHeight, topHeight);

            transform.position = Vector3.up * (topHeight - minHeight);
        }


        private void FixedUpdate()
        {
            UpdateTrigger();
        }


        private void OnTriggerEnter(Collider other)
        {
            OnPlayerEnter?.Invoke();
        }
    }
}
