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

        public void LookAt(Vector3 point)
        {
            point.z = 0;

            transform.position = point + offset;
        }

        public void Bind(IPlayer player)
        {
            this.player = player;
        }
        public void Unbind()
        {
            player = null;
        }

        private void FixedUpdate()
        {
            if (player == null)
                return;

            Vector3 playerPosition = player.Transform.position;
            playerPosition.x = 0;

            transform.position = Vector3.Lerp(transform.position, playerPosition + offset, speed);
        }
    }
}
