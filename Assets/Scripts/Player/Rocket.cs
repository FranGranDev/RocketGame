using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Data;

namespace Game.Player
{
    public class Rocket : MonoBehaviour, IPlayer
    {
        [Header("Values")]
        [SerializeField] private float speed;
        [Header("Components")]
        [SerializeField] private new Rigidbody rigidbody;
       
        [Inject(Id = "SizeX")]
        private float pipeMaxSize;



        private bool isDriving;
        private float moveDeltaX;

        public Transform Transform
        {
            get => transform;
        }


        public void Initialize(float speed)
        {
            this.speed = speed;
        }

        public void EngineOn()
        {
            rigidbody.isKinematic = true;

            isDriving = true;
        }
        public void EngineOff()
        {
            rigidbody.isKinematic = false;

            isDriving = false;
        }
        public void Turn(float deltaX)
        {
            
        }


        public void Drive()
        {
            //rigidbody.MovePosition(Vector3.up *)
        }


        private void FixedUpdate()
        {
            if(isDriving)
            {
                Drive();
            }

            transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
        }
    }
}
