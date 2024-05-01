using UnityEngine;

public class FSLoadingPool
{
    private FSLoading.Pool _pool;

    public FSLoadingPool(FSLoading.Pool pool)
    {
        _pool = pool;
    }

    public FSLoading Spawn(Transform parent)
    {
        return _pool.Spawn(parent);
    }

    public void Remove(FSLoading loading)
    {
        _pool.Despawn(loading);
    }
}