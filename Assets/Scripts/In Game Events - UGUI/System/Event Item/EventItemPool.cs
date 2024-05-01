using UnityEngine;

public class EventItemPool
{
    private EventItemUI.Pool _pool;

    public EventItemPool(EventItemUI.Pool pool)
    {
        _pool = pool;
    }

    public EventItemUI Spawn(Transform parent, EventsData data)
    {
        return _pool.Spawn(parent, data);
    }

    public void Remove(EventItemUI item)
    {
        _pool.Despawn(item);
    }
}