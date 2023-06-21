using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Enviroment;
using Game.Player;
using Game.Services;
using System;

namespace Game.Context
{
    public class GameController : MonoBehaviour, IGameEvents
    {
        [Header("Components")]
        [SerializeField] private Transform playerHolder;


        [Inject]
        private InfinityPipeCreator pipeCreator;

        [Inject]
        private ICamera<IPlayer> playerCamera;

        [Inject]
        private IFactory<Vector3, Transform, IPlayer> playerFactory;


        private IPlayer player;



        public event Action<GameStates> OnChanged;


        private void Start()
        {
            player = playerFactory.Create(playerHolder.position, playerHolder);

            pipeCreator.Initialize();
            pipeCreator.SetTarget(player.Transform);

            playerCamera.Bind(player);
        }
    }
}