using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineHandler : MonoBehaviour
{
    [SerializeField] private GameObject finishLine;
    [SerializeField] private float timeFinishLineSpawns = 2f;
    
    private Vector3 _startPosition;
    
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _startPosition = finishLine.transform.position;
        
        _gameManager.OnGameRestart += Restart;
    }

    private void Restart()
    {
        finishLine.transform.position = _startPosition;
        finishLine.gameObject.SetActive(false);
        GameManager.IsGameEnding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.TimeRemaining <= timeFinishLineSpawns)
            SpawnFinishLine();
    }
    
    private void SpawnFinishLine()
    {
        if (finishLine.gameObject.activeSelf)
            return;
        finishLine.gameObject.SetActive(true);
        GameManager.IsGameEnding = true;
    }
}
