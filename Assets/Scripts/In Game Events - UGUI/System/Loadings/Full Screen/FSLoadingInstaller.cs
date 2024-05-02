using UnityEngine;
using Zenject;

/// <summary>
/// Zenject Installer attach to the scene context
/// </summary>
public class FSLoadingInstaller : MonoInstaller
{
    /// <summary>
    /// Loading to spawn
    /// </summary>
    public GameObject _loader;

    public override void InstallBindings()
    {
        // binding the pool
        Container.Bind<FSLoadingPool>().AsSingle();
        // spawning item from the pool
        Container.BindMemoryPool<FSLoading, FSLoading.Pool>().WithInitialSize(1).FromComponentInNewPrefab(_loader).UnderTransformGroup("FSLoading");
    }
}