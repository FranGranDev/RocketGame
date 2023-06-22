using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public interface ICamera<T>
    {
        void LookAt(Vector3 point);

        void Bind(T player);
        void Unbind();
    }
}
