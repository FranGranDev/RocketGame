using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Difficult")]
    public class DifficultSettings : ScriptableObject
    {
        [SerializeField] private float maxHeight;
        [SerializeField] private float safeHeight;
        [SerializeField] private AnimationCurve difficultCurve;
        [Space]
        [SerializeField, Range(0, 1)] private float minPipesFrequency;
        [SerializeField, Range(0, 1)] private float maxPipesFrequency;
        [Space]
        [SerializeField, Range(0, 2)] private float obstacleSizeMultiplier;

        public float GetDifficult(float height)
        {
            if (height < safeHeight)
                return 0;
            return difficultCurve.Evaluate(Mathf.Clamp01(height / maxHeight));
        }

        public float MinPipesFrequency => minPipesFrequency;
        public float MaxPipesFrequency => maxPipesFrequency;

        public float ObstacleMultiplier => obstacleSizeMultiplier;
    }
}
