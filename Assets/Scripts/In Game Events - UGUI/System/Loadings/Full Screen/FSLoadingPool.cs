using UnityEngine;

/// <summary>
/// Pool for the loading
/// </summary>
public class FSLoadingPool
{
    private FSLoading.Pool _pool;

    /// <summary>
    /// Constructor and Memory Pool is passed as an argument to cache
    /// </summary>
    /// <param name="pool">Pool to use</param>
    public FSLoadingPool(FSLoading.Pool pool)
    {
        _pool = pool;
    }

    /// <summary>
    /// Calls when need to spawn FS Loading
    /// </summary>
    /// <param name="parent">what is the parent of this UI element</param>
    /// <returns>the reference of the loading</returns>
    public FSLoading Spawn(Transform parent)
    {
        return _pool.Spawn(parent);
    }

    /// <summary>
    /// Calls when need to remove or despawn
    /// </summary>
    /// <param name="loading">item to despawn</param>
    public void Remove(FSLoading loading)
    {
        _pool.Despawn(loading);
    }
}