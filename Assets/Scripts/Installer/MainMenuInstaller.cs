using DefaultNamespace.Progress;
using DefaultNamespace.Progress.Settings;
using DefaultNamespace.ProgressBar;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Installer
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private Hud _hud;
        [SerializeField] private ProgressSettings _progressSettings;
        [SerializeField] private ProgressElementView _progressElementView;

        public override void InstallBindings()
        {
            Container.Bind<Hud>()
                .FromComponentInNewPrefab(_hud)
                .AsSingle()
                .NonLazy();

            ProgressInstaller();
            ProgressBarInstaller();
        }

        private void ProgressInstaller()
        {
            Container.Bind<IProgressElementsCreator>()
                .To<ProgressElementsCreator>()
                .AsSingle();

            Container.Bind<IProgressElementsHandler>()
                .To<ProgressElementsHandler>()
                .AsSingle();

            Container.Bind(typeof(IProgressCounter), typeof(IProgressCounterPause), typeof(IProgressCounterIncrease))
                .To<ProgressCounter>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("ProgressCounter")
                .AsSingle();

            Container.Bind<ProgressSettings>()
                .FromScriptableObject(_progressSettings)
                .AsSingle();
        }

        private void ProgressBarInstaller()
        {
            Container.BindFactory<Transform, ProgressElement, ProgressElementView, ProgressElementView.Factory>()
                .FromComponentInNewPrefab(_progressElementView);
        }
    }
}