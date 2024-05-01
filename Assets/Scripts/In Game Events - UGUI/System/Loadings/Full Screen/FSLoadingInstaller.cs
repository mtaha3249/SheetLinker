using UnityEngine;
using Zenject;

public class FSLoadingInstaller : MonoInstaller
{
    public GameObject _loader;

    public override void InstallBindings()
    {
        Container.Bind<FSLoadingPool>().AsSingle();
        Container.BindMemoryPool<FSLoading, FSLoading.Pool>().WithInitialSize(1).FromComponentInNewPrefab(_loader).UnderTransformGroup("FSLoading");
    }
}