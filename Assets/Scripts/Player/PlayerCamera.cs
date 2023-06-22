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

        private float topHeight;


        public void LookAt(Vector3 point)
        {
            point.z = 0;

            transform.position = point + offset;
        }

        public void Bind(IPlayer player)
        {
            this.player = player;
            topHeight = float.MinValue;
        }
        public void Unbind()
        {
            player = null;
        }

        private void FixedUpdate()
        {
            if (player == null)
                return;
            topHeight = Mathf.Max(topHeight, player.Transform.position.y);

            Vector3 position = Vector3.up * topHeight;

            transform.position = Vector3.Lerp(transform.position, position + offset, speed);
        }
    }
}
