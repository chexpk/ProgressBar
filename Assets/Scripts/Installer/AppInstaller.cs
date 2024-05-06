using System;
using DefaultNamespace.Progress;
using DefaultNamespace.System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Installer
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField] private DefaultDataSettings _defaultDataSettings;

        public override void InstallBindings()
        {
            Container.Bind<IDefaultDataCreator>()
                .To<DefaultDataCreator>()
                .AsSingle();

            Container.Bind(typeof(ISaveSystem))
                .To<SaveSystem>()
                .AsSingle()
                .NonLazy();

            Container.Bind(typeof(ISaver))
                .To<SaveObserver>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("SaveObserver")
                .AsSingle()
                .NonLazy();

            Container.Bind<DefaultDataSettings>()
                .FromInstance(_defaultDataSettings)
                .AsSingle();
        }
    }
}