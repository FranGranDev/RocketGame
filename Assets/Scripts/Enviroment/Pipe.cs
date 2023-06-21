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

        public bool Hidden
        {
            get => !gameObject.activeSelf;
        }


        public void Show(float height, float difficult, float xRatio)
        {
            transform.position = Vector3.up * height;

            SetObstacles(difficult, xRatio);

            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }


        private void SetObstacles(float difficult, float xRatio)
        {
            float leftX = Mathf.Lerp(0, maxOffset, offsetCurve.Evaluate(difficult * xRatio));
            float rightX = Mathf.Lerp(0, maxOffset, offsetCurve.Evaluate(difficult * (1 - xRatio)));

            leftObstacle.localPosition = new Vector3(leftX, 0, 0);
            rightObstacle.localPosition = new Vector3(-rightX, 0, 0);
        }
    }
}