using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using System.Linq;
using Zenject;
using NaughtyAttributes;


namespace Game.Enviroment
{
    public class InfinityPipeCreator : MonoBehaviour
    {
        [Foldout("Difficult Settings"), SerializeField] private DifficultTypes difficultType;
        [Foldout("Difficult Settings"), SerializeField] private AnimationCurve xDiffCurve;
        [Foldout("Difficult Settings"), SerializeField] private int seed;
        [Foldout("Difficult Settings"), SerializeField] private bool autoSeed;

        [Foldout("Create Settings"), SerializeField] private float createOffset;
        [Foldout("Create Settings"), Space, SerializeField] private float createThreshold;
        [Foldout("Create Settings"), SerializeField] private float hideThreshold;

        [Foldout("Components"), SerializeField] private Transform container;


        [Inject]
        private IFactory<Pipe> pipeFactory;
        [Inject]
        private DifficultSettings difficultSettings;


        private PipesPool pipesPool;
        private IDifficult difficult;

        private Transform target;

        private Dictionary<int, Pipe> pipes = new Dictionary<int, Pipe>();
        private int currantIndex;
        private int prevIndex = int.MinValue;

        private int createOffsetIndex;
        private int hideOffsetIndex;

        public float CurrantHeight
        {
            get
            {
                return target.position.y;
            }
        }
        public float NormalizedHeight(int i)
        {
            return i * createOffset;
        }


        public void Initialize()
        {
            createOffsetIndex = Mathf.RoundToInt(createThreshold / createOffset);
            hideOffsetIndex = Mathf.RoundToInt(hideThreshold / createOffset) + createOffsetIndex;

            pipesPool = new PipesPool(pipeFactory, container, hideOffsetIndex * 3);

            if(autoSeed)
            {
                seed = Random.Range(0, int.MaxValue);
            }

            switch(difficultType)
            {
                case DifficultTypes.SmartRandom:
                    difficult = new SmartRandomDifficult(difficultSettings, seed);
                    break;
                default:
                    difficult = new RandomDifficult(difficultSettings, seed);
                    break;
            }
        }
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        public void Restart()
        {
            if (autoSeed)
            {
                seed = Random.Range(0, int.MaxValue);
            }

            pipesPool.Reset();
            pipes.Clear();
        }


        private void UpdatePipes()
        {
            currantIndex = Mathf.RoundToInt(CurrantHeight / createOffset);

            if (currantIndex == prevIndex)
            {
                return;
            }
            prevIndex = currantIndex;

            CreatePipes();
            FixImposiblePipes();
            HidePipes();
        }
        private void CreatePipes()
        {
            for (int i = -createOffsetIndex; i < createOffsetIndex; i++)
            {
                if (pipes.ContainsKey(currantIndex + i))
                {
                    ShowPipe(currantIndex + i);
                    continue;
                }

                CreatePipe(currantIndex + i);
            }
        }
        private void FixImposiblePipes()
        {
            for (int i = -createOffsetIndex; i < createOffsetIndex - 1; i++)
            {
                Pipe lower = pipes[currantIndex + i];
                Pipe upper = pipes[currantIndex + i + 1];


                if(lower.Difficult + upper.Difficult > 1f)
                {
                    if(lower.Difficult > upper.Difficult)
                    {
                        upper.Difficult = 0;
                    }
                    else
                    {
                        lower.Difficult = 0;
                    }
                }
            }
        }
        private void HidePipes()
        {
            for (int i = -hideOffsetIndex; i < hideOffsetIndex; i++)
            {
                if (i > -createOffsetIndex && i < createOffsetIndex)
                    continue;

                if (!pipes.ContainsKey(currantIndex + i))
                    continue;

                HidePipe(currantIndex + i);
            }
        }


        private void CreatePipe(int index)
        {
            pipes.Add(index, pipesPool.Get());

            ShowPipe(index);
        }
        private void HidePipe(int index)
        {
            pipes[index].Hide();

            pipes.Remove(index);
        }
        private void ShowPipe(int index)
        {
            float normalizedHeight = NormalizedHeight(index);
            float xRatio = xDiffCurve.Evaluate(Mathf.PerlinNoise(seed / 2, normalizedHeight));
            float difficultResult = difficult.Get(normalizedHeight);


            pipes[index].Show(normalizedHeight, difficultResult, xRatio);            
        }
        


        private void Update()
        {
            if (!target)
                return;
            UpdatePipes();
        }


        private class PipesPool
        {
            public PipesPool(IFactory<Pipe> factory, Transform container, int count)
            {
                pipes = new List<Pipe>(count);

                for(int i = 0; i < count; i++)
                {
                    Pipe pipe = factory.Create();
                    pipe.Hide();
                    pipe.transform.parent = container;

                    pipes.Add(pipe);
                }
            }

            private List<Pipe> pipes { get; }       
            
            public Pipe Get()
            {
                try
                {
                    return pipes.First(x => x.Hidden);
                }
                catch
                {
                    return pipes.First(); // If move infinity fast, that happen :)
                }
            }
            public void Reset()
            {
                pipes.ForEach(x => x.Hide());
            }
        }
    }
}
