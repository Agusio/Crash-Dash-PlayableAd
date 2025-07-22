using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed, despawnTime = 3f;
    [SerializeField] private bool shouldDestroy = true;
    
    
    private void Start()
    {
        //Adds to the event OnGameRestart the function of returning this object to the pool.
        GameManager.Instance.OnGameRestart += () => EnemyFactory.Instance.ReturnObjectToPool(this);
    }

    private void OnEnable()
    {
        if (!shouldDestroy) return;
        StartCoroutine(DestroyRoutine());
    }

    private void Update()
    {
        if (GameManager.IsPaused) return;
        
        transform.Translate(Vector3.back * (Time.deltaTime * speed));
    }

    //Instead of using Destroy(this, despawnTime), returns the object to the pool. 
    private IEnumerator DestroyRoutine()
    {
        var timer = despawnTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        
        if (GameManager.IsPaused) yield break; //Break prevents objects from disappearing when a GameOver screen is showing.
        EnemyFactory.Instance.ReturnObjectToPool(this);
    }
}
