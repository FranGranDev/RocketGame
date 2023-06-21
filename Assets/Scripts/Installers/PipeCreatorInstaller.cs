using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Enviroment;


namespace Game.Installers
{
    public class PipeCreatorInstaller : MonoInstaller
    {
        [SerializeField] private InfinityPipeCreator pipeCreator;

        public override void InstallBindings()
        {
            Container.Bind<InfinityPipeCreator>()
                .FromInstance(pipeCreator);
        }
    }
}
