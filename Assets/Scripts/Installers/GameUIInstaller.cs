using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI;

namespace Game.Installers
{
    public class GameUIInstaller : MonoInstaller
    {
        [SerializeField] private GameUI gameUI;

        public override void InstallBindings()
        {
            Container.Bind<IGameUI>()
                .To<GameUI>()
                .FromInstance(gameUI)
                .NonLazy();           
        }
    }
}
