using DefaultNamespace.Progress;
using Zenject;

namespace DefaultNamespace.Installer
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IProgressElementsCreator>()
                .To<ProgressElementsCreator>()
                .AsSingle();
        }
    }
}