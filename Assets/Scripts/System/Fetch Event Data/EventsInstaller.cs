using System.Collections.Generic;
using Zenject;

public class EventsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IFetchData<List<EventsData>>>().To<FetchEventData>().AsSingle();
    }
}
