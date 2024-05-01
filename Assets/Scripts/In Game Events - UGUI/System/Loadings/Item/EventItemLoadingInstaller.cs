using UnityEngine;
using Zenject;

public class EventItemLoadingInstaller : MonoInstaller
{
    public GameObject _loader;

    public override void InstallBindings()
    {
        Container.Bind<EventItemLoadingPool>().AsSingle();
        Container.BindMemoryPool<EventItemLoading, EventItemLoading.Pool>().WithInitialSize(10).FromComponentInNewPrefab(_loader).UnderTransformGroup("Item Loader");
    }
}