using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy prefab, finishLine;
    [SerializeField] private Transform[] spawnPoints;
    private float _timer; 
    [SerializeField] private float spawnInterval;
    private float _finishLineTimer;
    private Vector3 _finishLinePosition;
    private EnemyFactory _factory;

    private void Start()
    {
        _finishLinePosition = finishLine.transform.position;
        _timer = spawnInterval;
        _factory = EnemyFactory.Instance;
        GameManager.Instance.OnGameRestart += Restart;
    }

    private void Restart()
    {
        finishLine.gameObject.SetActive(true);
        finishLine.transform.position = _finishLinePosition;
        finishLine.gameObject.SetActive(false);
        _timer = 0;
        _finishLineTimer = 0;
    }

    private void Spawn(int iterations, List<Vector3> positions)
    {
        if (iterations == 0)
            return;
        if (positions.Count != iterations)
            return;
        for (int i = 0; i < iterations; i++)
        {
            var enemy = _factory.GetObjectFromPool();
            enemy.transform.position = positions[i];
        }
        
    }

    public void Update()
    {
        if (GameManager.IsPaused) return;
        
        if (_finishLineTimer >= 18)
        {
            if (finishLine.gameObject.activeSelf)
                return;
            finishLine.gameObject.SetActive(true);
            return;
        }
        
        _finishLineTimer += Time.deltaTime;

        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            var iterations = Random.Range(1, spawnPoints.Length);
            var positions = new List<Vector3>();
            Vector3 lastAddedPosition;
            for (int i = 0; i < iterations; i++)
            {
                var randomIndex = Random.Range(0, spawnPoints.Length);
                lastAddedPosition = spawnPoints[randomIndex].position;
                if (positions.Contains(lastAddedPosition))
                    if (randomIndex >= spawnPoints.Length)
                        randomIndex = 0;
                lastAddedPosition = spawnPoints[randomIndex].position;
                positions.Add(lastAddedPosition);
            }
                
            Spawn(iterations, positions);
            _timer = spawnInterval;
        }
    }
}
