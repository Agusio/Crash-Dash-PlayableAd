using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform[] positions;
    [SerializeField] private AudioSource crashAudio;
    [SerializeField] private GameObject particles;
    
    private int _index = 1;
    private Vector3 _currentPos;
    
    [SerializeField] private float turnAngle = 15f, turnDuration = 0.1f;

    private Coroutine _coroutine, _smoothCoroutine; //Saving coroutines for later using StopCoroutine() accordingly.
    private void Start()
    {
        _currentPos = positions[_index].position;
        crashAudio.time = 0.5f; //Sets audio accordingly.
        GameManager.Instance.OnGameOver += GameOver;
        GameManager.Instance.OnGameRestart += Restart;
    }

    /// <summary>
    /// Sets all values to default, preventing transform, audio, and particles unwanted behaviours.
    /// </summary>
    private void Restart()
    {
        _index = 1;
        _currentPos = positions[_index].position;
        transform.position = _currentPos;
        transform.rotation = Quaternion.identity;
        crashAudio.Stop();
        crashAudio.time = 0.5f;
        particles.SetActive(false);
    }

    /// <summary>
    /// Stops coroutine playing and sets Audio and particles off if the game was lost.
    /// </summary>
    /// <param name="win">true means victory, false means defeat</param>
    private void GameOver(bool win)
    {
        if (_smoothCoroutine != null) StopCoroutine(_smoothCoroutine);
        if (_coroutine != null) StopCoroutine(_coroutine);
        if (win) return; 
        particles.SetActive(true);
        crashAudio.Play();
    }

    /// <summary>
    /// Moves the position where the vehicle is at.
    /// If key is pressed to the direction where there is no valid position, it does nothing.
    /// </summary>
    /// <param name="direction">Left is -1, Right is 1</param>
    public void Move(int direction)
    {
        _index += direction;
        if (_index >= positions.Length || _index < 0)
        {
            _index = _index < 0 ? 0 : positions.Length - 1;
            return;
        }
        
        _currentPos = positions[_index].position;
        
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(MoveRoutine(turnAngle*direction, _currentPos));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            //False means defeat
            GameManager.Instance.OnGameOver(false);
        
    }

    #region Coroutines
    private IEnumerator MoveRoutine(float targetAngle, Vector3 targetPos)
    {
        var startPos = transform.position;
        var startRot = transform.rotation;
        var targetRot = Quaternion.Euler(0, targetAngle, 0);

        if (_smoothCoroutine != null) StopCoroutine(_smoothCoroutine);
        yield return _smoothCoroutine = StartCoroutine(SmoothMovementRoutine(startRot, targetRot, startPos, targetPos, true));
        if (_smoothCoroutine != null) StopCoroutine(_smoothCoroutine);
        yield return _smoothCoroutine = StartCoroutine(SmoothMovementRoutine(targetRot, Quaternion.identity));
    }
    
    /// <param name="startAngle">required, start angle of object</param>
    /// <param name="targetAngle">required, desired angle to look at</param>
    /// <param name="startPos">not required, start position of object</param>
    /// <param name="targetPos">not required, desired position to reach</param>
    /// <param name="shouldMove"></param>
    /// <returns></returns>
    private IEnumerator SmoothMovementRoutine(Quaternion startAngle, Quaternion targetAngle,
        Vector3 startPos = new(), Vector3 targetPos = new(), bool shouldMove = false)
    {
        var time = 0f;
        while (time < turnDuration)
        {
            time += Time.deltaTime;
            var progress = time / turnDuration;
            if (shouldMove) 
                transform.position = Vector3.Lerp(startPos, targetPos, progress);
            transform.rotation = Quaternion.Slerp(startAngle, targetAngle, progress);
            yield return null;
        }
    }
    #endregion
}
