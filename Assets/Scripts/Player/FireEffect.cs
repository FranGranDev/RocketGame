using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class FireEffect : MonoBehaviour, IEffect
    {
        [SerializeField] private List<ParticleSystem> particles;

        private List<IEffect> effects;

        public float Power
        {
            set
            {
                foreach(IEffect effect in effects)
                {
                    effect.Power = value;
                }
            }
        }

        private void Awake()
        {
            effects = new List<IEffect>(particles.Count);

            foreach(ParticleSystem particle in particles)
            {
                effects.Add(new Particle(particle));
            }
        }

        private class Particle : IEffect
        {
            public Particle(ParticleSystem particleSystem)
            {
                emission = particleSystem.emission;

                timeEmmission = emission.rateOverTime.constant;
                distanceEmmission = emission.rateOverDistance.constant;
            }

            private ParticleSystem.EmissionModule emission;

            private float timeEmmission;
            private float distanceEmmission;


            public float Power
            {
                set
                {
                    emission.rateOverDistance = Mathf.Lerp(0, distanceEmmission, value);
                    emission.rateOverTime = Mathf.Lerp(0, timeEmmission, value);
                }
            }
        }
    }
}
