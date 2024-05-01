using UnityEngine;
using Zenject;

public class EventItemInstaller : MonoInstaller
{
    public GameObject _eventItem;

    public override void InstallBindings()
    {
        Container.Bind<EventItemPool>().AsSingle();
        Container.BindMemoryPool<EventItemUI, EventItemUI.Pool>().WithInitialSize(20).FromComponentInNewPrefab(_eventItem).UnderTransformGroup("EventItems");
    }
}