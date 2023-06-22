using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Enviroment
{
    public class Pipe : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private AnimationCurve offsetCurve;
        [SerializeField] private float maxOffset;
        [Header("Components")]
        [SerializeField] private Transform leftObstacle;
        [SerializeField] private Transform rightObstacle;

        private float xRatio;
        private float difficult;


        public bool Hidden
        {
            get => !gameObject.activeSelf;
        }
        public float Difficult
        {
            get => difficult;
            set
            {
                difficult = value;
                SetObstacles();
            }
        }

        
        public void Show(float height, float difficult, float xRatio)
        {
            this.xRatio = xRatio;
            this.difficult = difficult;

            transform.position = Vector3.up * height;

            SetObstacles();

            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }


        private void SetObstacles()
        {
            float leftX = Mathf.Lerp(0, maxOffset, offsetCurve.Evaluate(difficult * xRatio));
            float rightX = Mathf.Lerp(0, maxOffset, offsetCurve.Evaluate(difficult * (1 - xRatio)));

            leftObstacle.localPosition = new Vector3(leftX, 0, 0);
            rightObstacle.localPosition = new Vector3(-rightX, 0, 0);
        }
    }
}