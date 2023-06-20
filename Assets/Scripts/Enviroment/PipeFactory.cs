using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Data;


namespace Game.Enviroment
{
    public class PipeFactory : IFactory<Pipe>
    {
        [Inject]
        private PipeData pipeData;

        [Inject]
        private DiContainer container;


        public Pipe Create()
        {
            return container.InstantiatePrefabForComponent<Pipe>(pipeData.Pipe);
        }
    }
}
