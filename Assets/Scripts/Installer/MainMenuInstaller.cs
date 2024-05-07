using DefaultNamespace.Progress;
using DefaultNamespace.Progress.Settings;
using DefaultNamespace.ProgressBar;
using DefaultNamespace.Reward;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Installer
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private Hud _hud;
        [SerializeField] private ProgressSettings _progressSettings;
        [SerializeField] private ProgressElementView _progressElementView;
        [SerializeField] private RewardView _rewardView;

        public override void InstallBindings()
        {
            Container.Bind<Hud>()
                .FromComponentInNewPrefab(_hud)
                .AsSingle()
                .NonLazy();

            ProgressInstaller();
            ProgressBarInstaller();
            RewardInstaller();
        }

        private void ProgressInstaller()
        {
            Container.Bind(typeof(IInitializable), typeof(IProgressElementsHandler))
                .To<ProgressElementsHandler>()
                .AsSingle();

            Container.Bind(typeof(IProgressCounter), typeof(IProgressCounterIncrease),
                    typeof(ITickable), typeof(IInitializable))
                .To<ProgressCounter>()
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

        private void RewardInstaller()
        {
            Container.BindFactory<Transform, ProgressElement, RewardView, RewardView.Factory>()
                .FromComponentInNewPrefab(_rewardView);

            Container.BindInterfacesTo<Rewarder>()
                .AsSingle()
                .NonLazy();
        }
    }
}