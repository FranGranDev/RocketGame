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
        public const int NOISE_MULTIPLIER = 1337;

        public DifficultBase(DifficultSettings settings)
        {
            this.settings = settings;
        }

        protected DifficultSettings settings;



        public abstract float Get(float height);
    }

    public class RandomDifficult : DifficultBase
    {
        public RandomDifficult(DifficultSettings settings, int randomize) : base(settings)
        {
            this.randomize = randomize;
        }

        private int randomize;

        public override float Get(float height)
        {
            float result = Mathf.PerlinNoise(randomize, height * NOISE_MULTIPLIER) * settings.GetDifficult(height);
            return result < 0.5f ? 0 : result;
        }
    }
    public class SmartRandomDifficult : DifficultBase
    {
        public SmartRandomDifficult(DifficultSettings settings, int randomize) : base(settings)
        {
            this.randomize = randomize;
        }

        private int randomize;

        public override float Get(float height)
        {
            float difficult = settings.GetDifficult(height);
            if (difficult == 0)
                return 0;

            float pipesFrequency = Mathf.Lerp(settings.MinPipesFrequency, settings.MaxPipesFrequency, difficult);

            float activateNoise = Mathf.Pow(Mathf.PerlinNoise(-randomize, height), 0.75f);

            if (activateNoise > pipesFrequency)
                return 0;

            return Mathf.Clamp01(Mathf.PerlinNoise(randomize, height * NOISE_MULTIPLIER) * Mathf.Sqrt(difficult) * settings.ObstacleMultiplier); 
        }
    }


    public enum DifficultTypes
    {
        SmartRandom,
    }
}
