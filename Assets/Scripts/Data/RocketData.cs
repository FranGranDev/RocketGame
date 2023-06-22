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


        public GameObject Prefab => prefab;
    }
}
