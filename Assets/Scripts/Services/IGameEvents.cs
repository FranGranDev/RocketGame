using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public interface IGameEvents
    {
        public event Action<GameStates> OnChanged;
    }

    public enum GameStates
    {
        Idle,
        Game,
    }
}