using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public interface IEffect
    {
        public float Power { set; }
    }
    public class NullEffect : IEffect
    {
        public float Power { set; get; }
    }
}
