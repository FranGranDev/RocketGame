using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Data;
using System.Threading.Tasks;

namespace Game.Player
{
    public class Rocket : MonoBehaviour, IPlayer
    {
        [Header("Hit Settings")]
        [SerializeField, Range(0, 5)] private float loseControllTime;
        [Header("Gravity Settings")]
        [SerializeField, Range(-50, 0)] private float gravity;
        [Header("Fly Settings")]
        [SerializeField] private float maxSpeed;
        [SerializeField, Range(0.01f, 1f)] private float acceleration;
        [SerializeField, Range(0f, 1f)] private float directionRatio;
        [Header("Move Settings")]
        [SerializeField, Range(0.01f, 1f)] private float moveSmooth;
        [SerializeField] private float moveSpeed;
        [SerializeField, Range(0f, 1f)] private float xMoveSpeedRatio;
        [Header("Animation")]
        [SerializeField, Range(0f, 1f)] private float speedXfallAngularRatio;
        [SerializeField, Range(0f, 10f)] private float directionXfallAngular;
        [SerializeField, Range(0.01f, 1f)] private float turnSmooth;
        [SerializeField, Range(0.01f, 5f)] private float turnPower;
        [Header("Components")]
        [SerializeField] private new Rigidbody rigidbody;


        private IEffect effect;


        private bool isDriving;
        private float moveDirection;

        private bool noControll;


        public Transform Transform
        {
            get => transform;
        }
        public bool IsDriving
        {
            get => isDriving && !noControll;
        }


        public void Initialize()
        {
            effect = GetComponentInChildren<IEffect>();
            if(effect == null)
            {
                effect = new NullEffect();
            }

            rigidbody.velocity = Vector3.up * maxSpeed;
        }

        public void EngineOn()
        {
            rigidbody.angularVelocity = Vector3.zero;

            isDriving = true;
        }
        public void EngineOff()
        {
            float xPower = rigidbody.velocity.x * speedXfallAngularRatio + transform.up.x * directionXfallAngular;
            Vector3 torque = new Vector3(0, 0, -xPower);
            rigidbody.AddTorque(torque, ForceMode.VelocityChange);

            Vector3 velocity = rigidbody.velocity;
            velocity.x *= 0.5f;
            rigidbody.velocity = velocity;

            isDriving = false;
        }
        public void Move(Vector2 direction)
        {
            moveDirection = direction.x;
        }

        public void Demolish()
        {
            Destroy(gameObject);
        }

        private async void LoseControll()
        {
            if (noControll)
                return;
            noControll = true;
            await Task.Delay(Mathf.RoundToInt(loseControllTime * 1000));
            noControll = false;
        }


        private void Drive()
        {
            float currantAcceleration = acceleration * (rigidbody.velocity.y > 0 ? 1 : 0.5f);

            float velocityY = Mathf.Sqrt(Mathf.Max(maxSpeed * maxSpeed - rigidbody.velocity.x * rigidbody.velocity.x * xMoveSpeedRatio, 0));
            Vector3 targetVelocity = new Vector3(rigidbody.velocity.x, velocityY);
            targetVelocity = Vector3.Lerp(targetVelocity.normalized, transform.up, directionRatio) * targetVelocity.magnitude;

            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, targetVelocity, currantAcceleration);

            float velocityXRatio = Mathf.Abs(rigidbody.velocity.y / maxSpeed);
            rigidbody.AddForce(Vector3.right * moveDirection * moveSpeed * velocityXRatio, ForceMode.Acceleration);
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.up * rigidbody.velocity.y, moveSmooth);


            float velocityX = rigidbody.velocity.x;
            rigidbody.velocity = new Vector3(Mathf.Clamp(velocityX, -moveSpeed, moveSpeed), rigidbody.velocity.y, 0);
        }
        private void Animate()
        {
            Vector3 direction = new Vector3(rigidbody.velocity.x * turnPower, Mathf.Max(Mathf.Abs(rigidbody.velocity.y) * 0.75f, 1), 0).normalized;
            Quaternion rotation = Quaternion.LookRotation(transform.forward, direction);
            rotation.eulerAngles = new Vector3(0, 0, rotation.eulerAngles.z);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSmooth);
            rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, Vector3.zero, 0.1f);
        }
        private void Effects()
        {
            effect.Power = (IsDriving ? 1 : 0);
        }
        private void Gravity()
        {
            rigidbody.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
        }


        private void FixedUpdate()
        {
            if(IsDriving)
            {
                Drive();
                Animate();
            }
            else
            {
                Gravity();
            }

            Effects();
        }
        private void OnCollisionEnter(Collision collision)
        {
            LoseControll();
        }
    }
}
