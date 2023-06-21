using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Data
{
    [CreateAssetMenu(menuName = "Enviroment/Pipes")]
    public class PipeData : ScriptableObject
    {
        [SerializeField] private GameObject pipe;

        public GameObject Pipe
        {
            get => pipe;
        }
    }
}
