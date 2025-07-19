using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory<T>
{
    //[SerializeField] protected T _prefab;
    //[SerializeField] protected int _initialAmount;

    protected Pool<T> _pool;

    public Factory(/*T prefab, int initialAmount, */Pool<T> pool)
    {
        //_prefab = prefab;
        //_initialAmount = initialAmount;
        _pool = pool;
    }

    public T GetObjectFromPool()
    {
        return _pool.GetObject();
    }

    public void ReturnObjectToPool(T obj)
    {
        _pool.ReturnObjectToPool(obj);
    }
}