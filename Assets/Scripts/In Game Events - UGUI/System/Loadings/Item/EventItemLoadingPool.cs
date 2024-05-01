using UnityEngine;

public class EventItemLoadingPool
{
    private EventItemLoading.Pool _pool;

    public EventItemLoadingPool(EventItemLoading.Pool pool)
    {
        _pool = pool;
    }

    public EventItemLoading Spawn(Transform parent)
    {
        return _pool.Spawn(parent);
    }

    public void Remove(EventItemLoading loading)
    {
        _pool.Despawn(loading);
    }
}