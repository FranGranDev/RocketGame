using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public interface IPlayer : IDrivable
    {
        public Transform Transform { get; }
        public bool IsDriving { get; }
    }

    public interface IDrivable
    {
        void Move(Vector2 point);

        void EngineOn();
        void EngineOff();
    }
}
