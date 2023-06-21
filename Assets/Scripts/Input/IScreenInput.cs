using UnityEngine;
using System;


namespace Game.Inputs
{
    public interface IScreenInput
    {
        public event Action OnTap;
        public event Action OnRelease;

        public event Action<Vector2> OnDrag;
    }
}
