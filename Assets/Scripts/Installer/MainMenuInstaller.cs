using UnityEngine;
using Zenject;

namespace DefaultNamespace.Installer
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private Hud _hud;

        public override void InstallBindings()
        {
            Container.Bind<Hud>()
                .FromComponentInNewPrefab(_hud)
                .AsSingle()
                .NonLazy();
        }
    }
}