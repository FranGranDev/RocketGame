using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Player;


namespace Game.Installers
{
    public class RocketFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IFactory<Vector3, Transform, IPlayer>>()
                .To<RocketFactory>()
                .AsSingle()
                .NonLazy();
        }
    }
}
