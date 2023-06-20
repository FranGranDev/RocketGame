using UnityEngine;
using Zenject;
using Game.Data;
using Game.Enviroment;

namespace Game.Installers
{
    [CreateAssetMenu(fileName = "GameDataInstaller", menuName = "Installers/GameDataInstaller")]
    public class GameDataInstaller : ScriptableObjectInstaller<GameDataInstaller>
    {
        [SerializeField] private PipeData pipeData;

        public override void InstallBindings()
        {
            Container.Bind<PipeData>()
                .FromInstance(pipeData)
                .NonLazy();
        }
    }
}