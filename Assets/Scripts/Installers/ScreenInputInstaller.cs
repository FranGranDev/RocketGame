using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Inputs;

namespace Game.Installers
{
    public class ScreenInputInstaller : MonoInstaller
    {
        [SerializeField] private ScreenInput screenInput;

        public override void InstallBindings()
        {
            Container.Bind<IScreenInput>()
                .To<ScreenInput>()
                .FromInstance(screenInput)
                .NonLazy();
        }
    }
}
