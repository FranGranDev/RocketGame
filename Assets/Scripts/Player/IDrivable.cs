using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public interface IPlayer : IDrivable
    {
        public Transform Transform { get; }
    }

    public interface IDrivable
    {
        void Turn(float deltaX);

        void EngineOn();
        void EngineOff();
    }
}
