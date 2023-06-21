using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerCamera : MonoBehaviour, ICamera<IPlayer>
    {
        [Header("Settings")]
        [SerializeField, Range(0.05f, 1f)] private float speed;
        [SerializeField] private Vector3 offset;

        private IPlayer player;

        public void Bind(IPlayer player)
        {
            this.player = player;
        }

        private void FixedUpdate()
        {
            if (player == null)
                return;

            Vector3 targetPosition = player.Transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
        }
    }
}
