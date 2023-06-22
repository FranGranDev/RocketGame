using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Enviroment;
using Game.Player;
using Game.Services;
using UI;
using System;
using System.Threading.Tasks;

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
        private IGameUI gameUI;

        [Inject]
        private IFactory<Vector3, Transform, IPlayer> playerFactory;


        private IPlayer player;
        private GameStates gameState;


        public GameStates GameState
        {
            get => gameState;
            set
            {
                if (GameState == value)
                    return;

                gameState = value;
                OnChanged?.Invoke(gameState);
            }
        }
        public event Action<GameStates> OnChanged;


        private async void Initialize()
        {
            pipeCreator.Initialize();
            pipeCreator.SetTarget(playerHolder);

            gameUI.OnStartClick += StartGame;
            gameUI.OnRestartClick += RestartGame;

            await Task.Delay(1000);

            OnChanged?.Invoke(GameStates.Idle);
        }

        private void RestartGame()
        {
            pipeCreator.SetTarget(playerHolder);
            pipeCreator.Restart();
            playerCamera.Unbind();
            playerCamera.LookAt(playerHolder.position);

            player.Delete();

            GameState = GameStates.Idle;
        }

        private void StartGame()
        {
            player = playerFactory.Create(playerHolder.position, playerHolder);

            pipeCreator.SetTarget(player.Transform);
            playerCamera.Bind(player);


            GameState = GameStates.Game;
        }

        private void Awake()
        {
            Initialize();
        }
    }
}