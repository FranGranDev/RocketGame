using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Enviroment;
using Game.Player;


namespace Game.Context
{
    public class GameController : MonoBehaviour
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


        private void Start()
        {
            player = playerFactory.Create(playerHolder.position, playerHolder);

            pipeCreator.Initialize();
            pipeCreator.SetTarget(player.Transform);

            playerCamera.Bind(player);
        }
    }
}