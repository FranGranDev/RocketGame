using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Player;


namespace Game.Installers
{
    public class PlayerCameraInstaller : MonoInstaller
    {
        [SerializeField] private PlayerCamera playerCamera;

        public override void InstallBindings()
        {
            Container.Bind<ICamera<IPlayer>>()
                .To<PlayerCamera>()
                .FromInstance(playerCamera)
                .NonLazy();
        }
    }
}
