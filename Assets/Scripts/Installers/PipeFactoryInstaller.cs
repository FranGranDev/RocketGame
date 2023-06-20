using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Enviroment;

namespace Game.Installers
{
    public class PipeFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IFactory<Pipe>>()
                .To<PipeFactory>()
                .AsSingle()
                .NonLazy();
        }
    }
}