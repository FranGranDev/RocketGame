using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enviroment
{
    public class Noise
    {
        public static float NewSeed()
        {
            return Random.Range(0f, 1f);
        }

        public Noise(float seed, float scale)
        {
            this.scale = scale;
            this.seed = seed;
        }


        private float scale;
        private float seed;

        public float Evaluate(float height)
        {
            float input = (float)height * scale;

            return Mathf.PerlinNoise(seed, input);
        }
    }
}
