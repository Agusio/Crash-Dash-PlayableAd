using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }
    
    [SerializeField] private int amount;
    [SerializeField] private Enemy prefab;

    private Factory<Enemy> _factory;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(this);
            return;
        }

        var pool = new Pool<Enemy>(
            () => Instantiate(prefab), 
            (enemy) => enemy.gameObject.SetActive(true),
            (enemy) => enemy.gameObject.SetActive(false),
            amount);
        _factory = new(pool);
    }

    public Enemy GetObjectFromPool()
    { 
        return _factory.GetObjectFromPool();
    }

    public void ReturnObjectToPool(Enemy obj)
    {
        _factory.ReturnObjectToPool(obj);
    }
}