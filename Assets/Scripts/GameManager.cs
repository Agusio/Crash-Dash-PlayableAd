using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameWonPanel, gameLostPanel, buttonsPanel;
    private float _timer;
    [SerializeField] private float maxTime;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private float fadeTime;
    
    /// <param name="bool">True means victory, false means defeat</param>
    public Action<bool> OnGameOver = delegate { };
    public Action OnGameRestart = delegate { };
    
    public static bool IsPaused { get; private set; }
    public static GameManager Instance { get; private set; }

    private void Awake() => Instance = this;
    public void RestartGame() => OnGameRestart?.Invoke();

    private void Start()
    {
        _timer = maxTime;
        OnGameOver += GameOver;
        OnGameRestart += Restart;
        IsPaused = false;
    }

    private void Update()
    {
        if (IsPaused) return;
        _timer -= Time.deltaTime;
        timerText.text = _timer.ToString("0.0");
        if (_timer <= 0)
            //True means victory
            OnGameOver?.Invoke(true);
    }

    private void Restart()
    {
        _timer = maxTime;
        IsPaused = false;
        gameLostPanel.SetActive(false);
        buttonsPanel.SetActive(true);
        gameWonPanel.SetActive(false);
    }
    
    private void GameOver(bool win)
    {
        IsPaused = true;
        if (win) gameWonPanel.SetActive(true);
        else gameLostPanel.SetActive(true);
        StartCoroutine(GameOverRoutine(win ? gameWonPanel.GetComponent<Image>() : gameLostPanel.GetComponent<Image>()));
        buttonsPanel.SetActive(false);
    }

    private IEnumerator GameOverRoutine(Image image)
    {
        var startColor = image.color;
        var startAlpha = image.color.a;
        var t = 0f;

        while (t < fadeTime)
        {
            var newAlpha = Mathf.Lerp(0, startAlpha, t / fadeTime);
            image.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            t += Time.deltaTime;
            yield return null;
        }

        image.color = startColor;
        yield return null;
    }
}
