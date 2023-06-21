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
        [SerializeField] private float speed;


        public GameObject Prefab => prefab;
        public float Speed => speed;
    }
}
