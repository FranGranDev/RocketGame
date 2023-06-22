using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace Game.Enviroment
{
    public interface IDifficult
    {
        public float Get(float height);        
    }

    public abstract class DifficultBase : IDifficult
    {
        public DifficultBase(DifficultSettings settings)
        {
            this.settings = settings;
        }

        protected DifficultSettings settings;


        public abstract float Get(float height);
    }

    public class SmartRandomDifficult : DifficultBase
    {
        public const float DIFFICULT_NOISE_SCALE = 1.33653f;
        public const float ACTIVATE_NOISE_SCALE = 132.33653f;

        public SmartRandomDifficult(DifficultSettings settings, float seed) : base(settings)
        {
            difficultNoise = new Noise(seed, DIFFICULT_NOISE_SCALE);
            activateNoise = new Noise(seed, ACTIVATE_NOISE_SCALE);
        }

        private Noise difficultNoise;
        private Noise activateNoise;


        public override float Get(float height)
        {
            float difficult = settings.GetDifficult(height);
            if (difficult == 0)
                return 0;

            float pipesFrequency = Mathf.Lerp(settings.MinPipesFrequency, settings.MaxPipesFrequency, difficult);

            float activation = activateNoise.Evaluate(height);

            if (activation > pipesFrequency)
                return 0;

            return Mathf.Clamp01(difficultNoise.Evaluate(height) * Mathf.Sqrt(difficult) * settings.ObstacleMultiplier); 
        }
    }


    public enum DifficultTypes
    {
        SmartRandom,
    }
}
