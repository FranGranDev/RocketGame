using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Data;


namespace Game.Player
{
    public class RocketFactory : IFactory<Vector3, Transform, IPlayer>
    {
        [Inject]
        private RocketData rocketData;

        [Inject]
        private DiContainer container;


        public IPlayer Create(Vector3 position, Transform parent)
        {
            Rocket rocket = container.InstantiatePrefabForComponent<Rocket>(rocketData.Prefab);
            rocket.transform.position = position;
            rocket.transform.parent = parent;

            rocket.Initialize(rocketData.Speed);

            return rocket;
        }
    }
}
