using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool shouldDestroy = true;
    
    private void Start()
    {
        if (!shouldDestroy) return;
        StartCoroutine(DestroyRoutine());
        GameManager.Instance.OnGameRestart += () => EnemyFactory.Instance.ReturnObjectToPool(this);
    }

    private void Update()
    {
        if (GameManager.IsPaused) return;
        transform.Translate(Vector3.back * (Time.deltaTime * speed));
    }

    private IEnumerator DestroyRoutine()
    {
        var timer = 5f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        if (GameManager.IsPaused) yield break;
        EnemyFactory.Instance.ReturnObjectToPool(this);
    }
}
