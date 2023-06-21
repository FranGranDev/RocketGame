using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public interface ICamera<T>
    {
        void Bind(T player);
    }
}
