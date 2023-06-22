using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Context;
using Zenject;
using Game.Services;

namespace Game.Installers
{
    public class GameEventInstaller : MonoInstaller
    {
        [SerializeField] private GameController gameController;

        public override void InstallBindings()
        {
            Container.Bind<IGameEvents>()
                .To<GameController>()
                .FromInstance(gameController)
                .NonLazy();
        }
    }
}
