using UnityEngine;

/// <summary>
/// Pool of the Event Item
/// </summary>
public class EventItemPool
{
    private EventItemUI.Pool _pool;

    /// <summary>
    /// Constructor of the Pool
    /// </summary>
    /// <param name="pool">Reference of the Memory Pool</param>
    public EventItemPool(EventItemUI.Pool pool)
    {
        _pool = pool;
    }

    /// <summary>
    /// Calls when spawn item from the pool
    /// </summary>
    /// <param name="parent">parent to be</param>
    /// <param name="data">data to be used</param>
    /// <returns></returns>
    public EventItemUI Spawn(Transform parent, EventsData data)
    {
        return _pool.Spawn(parent, data);
    }

    /// <summary>
    /// Calls when remove or Despawn the item
    /// </summary>
    /// <param name="item">itrem to remove</param>
    public void Remove(EventItemUI item)
    {
        _pool.Despawn(item);
    }
}