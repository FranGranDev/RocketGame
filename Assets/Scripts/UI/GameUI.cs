using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using Zenject;
using System;


namespace UI
{
    public interface IGameUI
    {
        public event Action OnStartClick;
        public event Action OnRestartClick;
    }

    public class GameUI : MonoBehaviour, IGameUI
    {
        [Header("Menus")]
        [SerializeField] private GameObject startMenu;
        [SerializeField] private GameObject gameMenu;
        [Header("Buttons")]
        [SerializeField] private ButtonUI startButton;
        [SerializeField] private ButtonUI restartButton;
        [Header("States")]
        [SerializeField] private GameStates gameState;

        private Dictionary<GameStates, List<UIPanel>> menuPanels;


        [Inject]
        private IGameEvents gameEvents;

        public GameStates GameState
        {
            get
            {
                return gameState;
            }
            set
            {
                if (gameState == value)
                    return;
                OnStateEnd(gameState);
                gameState = value;
                OnStateStart(gameState);
            }
        }


        public event Action OnStartClick;
        public event Action OnRestartClick;



        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            menuPanels = new Dictionary<GameStates, List<UIPanel>>()
            {
                {GameStates.Idle, startMenu.GetComponentsInChildren<UIPanel>(true).ToList() },
                {GameStates.Game, gameMenu.GetComponentsInChildren<UIPanel>(true).ToList() },
            };
            menuPanels.Values.SelectMany(x => x).ToList().ForEach(x => x.Initilize());

            startButton.OnClick += StartClick;
            restartButton.OnClick += RestartClick;

            gameEvents.OnChanged += OnGameStateChanged;
        }


        private void OnGameStateChanged(GameStates state)
        {
            GameState = state;
        }
        private void OnStateStart(GameStates state)
        {
            menuPanels[state].ForEach(x => x.IsShown = true);
        }
        private void OnStateEnd(GameStates state)
        {
            menuPanels[state].ForEach(x => x.IsShown = false);
        }



        private void RestartClick()
        {
            if (gameState != GameStates.Game)
                return;
            OnRestartClick?.Invoke();   
        }
        private void StartClick()
        {
            if (gameState != GameStates.Idle)
                return;
            OnStartClick?.Invoke();
        }
    }
}
