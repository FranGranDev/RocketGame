using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Rocket")]
    public class RocketData : ScriptableObject
    {
        [Header("Prefab")]
        [SerializeField] private GameObject prefab;
        [Header("Settings")]
        [SerializeField] private Settings settings;


        public GameObject Prefab => prefab;
        public Settings Setting => settings;


        public class Settings
        {
            [SerializeField] private float maxSpeed;
            [SerializeField] private float turnSpeed;
            [Space]
            [SerializeField, Range(0.01f, 1f)] private float acceleration;
            [SerializeField, Range(0.01f, 1f)] private float turnSmooth;

            public float MaxSpeed => maxSpeed;
            public float TurnSpeed => turnSpeed;

            public float Acceleration => acceleration;
            public float TurnSmooth => turnSmooth;
        }
    }
}
