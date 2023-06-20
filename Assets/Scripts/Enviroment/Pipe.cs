using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Enviroment
{
    public class Pipe : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float maxOffset;
        [Header("Components")]
        [SerializeField] private Transform leftObstacle;
        [SerializeField] private Transform rightObstacle;


        public void Setup(float height, float difficult)
        {
            transform.position = Vector3.up * height;


        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}