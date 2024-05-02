using UnityEngine;
using Zenject;

/// <summary>
/// Zenject Installer attach to the scene context
/// </summary>
public class EventItemInstaller : MonoInstaller
{
    /// <summary>
    /// Event Item to spawn
    /// </summary>
    public GameObject _eventItem;

    public override void InstallBindings()
    {
        // binding the pool
        Container.Bind<EventItemPool>().AsSingle();
        // spawning item from the pool
        Container.BindMemoryPool<EventItemUI, EventItemUI.Pool>().WithInitialSize(20).FromComponentInNewPrefab(_eventItem).UnderTransformGroup("EventItems");
    }
}