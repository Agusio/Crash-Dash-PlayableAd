using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy prefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 3f;
    private float _spawnerTimer;
    
    private EnemyFactory _factory;


    private void Start()
    {
        _spawnerTimer = spawnInterval;
        
        _factory = EnemyFactory.Instance;
        //Adds to the event OnGameRestart the function of setting to 0 spawnerTimer;
        GameManager.Instance.OnGameRestart += () => {_spawnerTimer = 0;};
    }
    
    public void Update()
    {
        if (GameManager.IsPaused) return;

        if (GameManager.IsGameEnding) return;
        
        _spawnerTimer -= Time.deltaTime;
        if (_spawnerTimer <= 0)
            SpawnLogic();
    }

    /// <summary>
    /// Spawns a number of enemies in randomly shuffled lane positions.
    /// Ensures no duplicate positions and always leaves at least one road available
    /// </summary>
    private void SpawnLogic()
    {
        //Number of spawned cars, ensuring there's always 1 road available.
        var lanesOccupied = Random.Range(1, spawnPoints.Length);
        
        // LINQ is more readable but decreases runtime performance.
        // var shuffledPositions = spawnPoints.OrderBy(x => Random.value).
        //     Take(lanesOccupied).
        //     Select(x => x.position).
        //     ToList();
        //     Spawn(lanesOccupied, shuffledPositions);

        //Instead using a custom Shuffle() for better runtime performance.
        var shuffledPositions = spawnPoints.ToList().Shuffle();
 
        var positions = new List<Vector3>();
        for (int i = 0; i < lanesOccupied; i++)
        {
            positions.Add(shuffledPositions[i].position);
        }
                
        Spawn(lanesOccupied, positions);
        _spawnerTimer = spawnInterval;
    }
    
    private void Spawn(int lanesOccupied, List<Vector3> positions)
    {
        // For safety: this should never happen unless spawn logic breaks.
        // Dev note: consider throwing an exception if debugging.
        if (lanesOccupied <= 0) return; 
        if (positions.Count != lanesOccupied) return;
        
        for (var i = 0; i < lanesOccupied; i++)
        {
            var enemy = _factory.GetObjectFromPool();
            enemy.transform.position = positions[i];
        }
    }
}