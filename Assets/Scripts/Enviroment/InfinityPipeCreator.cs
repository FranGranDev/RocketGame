using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Zenject;


namespace Game.Enviroment
{
    public class InfinityPipeCreator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float createOffset;
        [SerializeField] private float createThreshold;
        [SerializeField] private float hideThreshold;
        [SerializeField] private float deleteThreshold;
        [Header("Components")]
        [SerializeField] private Transform container;
        [SerializeField] private Transform target;

        [Inject]
        private IFactory<Pipe> pipeFactory;

        private Dictionary<int, Pipe> pipes = new Dictionary<int, Pipe>();
        private int currantIndex;

        private int createOffsetIndex;
        private int hideOffsetIndex;
        private int deleteOffsetIndex;


        private void CalculateOffsets()
        {
            createOffsetIndex = Mathf.RoundToInt(createThreshold / createOffset);
            hideOffsetIndex = Mathf.RoundToInt(hideThreshold / createOffset) + createOffsetIndex;
            deleteOffsetIndex = Mathf.RoundToInt(deleteThreshold / createOffset) + hideOffsetIndex;
        }


        private void UpdatePipes()
        {
            for(int i = -createOffsetIndex; i < createOffsetIndex; i++)
            {
                if (pipes.ContainsKey(currantIndex + i))
                {
                    ShowPipe(currantIndex + i);
                    continue;
                }

                CreatePipe(currantIndex + i);
            }

            for(int i = -hideOffsetIndex; i < hideOffsetIndex; i++)
            {
                if (i > -createOffsetIndex && i < createOffsetIndex)
                    continue;

                if (!pipes.ContainsKey(currantIndex + i))
                    continue;

                HidePipe(currantIndex + i);
            }

            for (int i = -deleteOffsetIndex; i < deleteOffsetIndex; i++)
            {
                if (i > -hideOffsetIndex && i < hideOffsetIndex)
                    continue;

                if (!pipes.ContainsKey(currantIndex + i))
                    continue;

                DeletePipe(currantIndex + i);
            }
        }
        private void CreatePipe(int index)
        {
            Pipe pipe = pipeFactory.Create();

            pipe.Setup(index * createOffset, 0);
            pipe.transform.parent = container;

            pipes.Add(index, pipe);
        }
        private void HidePipe(int index)
        {
            pipes[index].Hide();
        }
        private void ShowPipe(int index)
        {
            pipes[index].Show();
        }
        private void DeletePipe(int index)
        {
            pipes[index].Delete();

            pipes.Remove(index);
        }



        private void Start()
        {
            CalculateOffsets();
        }
        private void FixedUpdate()
        {
            currantIndex = Mathf.RoundToInt(target.position.y / createOffset);

            UpdatePipes();
        }
    }
}
