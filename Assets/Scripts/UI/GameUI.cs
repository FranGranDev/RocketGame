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
        [SerializeField] private States state;

        private Dictionary<States, List<UIPanel>> menuPanels;


        [Inject]
        private IGameEvents gameEvents;

        private States State
        {
            get
            {
                return state;
            }
            set
            {
                if (State == value)
                    return;
                OnStateEnd(state);
                state = value;
                OnStateStart(state);
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
            menuPanels = new Dictionary<States, List<UIPanel>>()
            {
                {States.None, new List<UIPanel>() },
                {States.Start, startMenu.GetComponentsInChildren<UIPanel>(true).ToList() },
                {States.Game, gameMenu.GetComponentsInChildren<UIPanel>(true).ToList() },
            };
            menuPanels.Values.SelectMany(x => x).ToList().ForEach(x => x.Initilize());

            startButton.OnClick += StartClick;
            restartButton.OnClick += RestartClick;

            gameEvents.OnChanged += OnGameStateChanged;

            TurnUI(true);
            State = States.None;
        }


        private void OnGameStateChanged(GameStates state)
        {
            switch(state)
            {
                case GameStates.Idle:
                    State = States.Start;
                    break;
                case GameStates.Game:
                    State = States.Game;
                    break;
                default:
                    State = States.None;
                    break;
            }
        }
        private void OnStateStart(States state)
        {
            menuPanels[state].ForEach(x => x.IsShown = true);
        }
        private void OnStateEnd(States state)
        {
            menuPanels[state].ForEach(x => x.IsShown = false);
        }

        private void TurnUI(bool on)
        {
            startMenu.SetActive(on);
            gameMenu.SetActive(on);
        }


        private void RestartClick()
        {
            if (state != States.Game)
                return;
            OnRestartClick?.Invoke();   
        }
        private void StartClick()
        {
            if (state != States.Start)
                return;
            OnStartClick?.Invoke();
        }

        private enum States
        {
            None,
            Start,
            Game,
        }
    }
}
