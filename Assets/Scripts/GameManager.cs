using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameWonPanel, gameLostPanel, buttonsPanel;
    [SerializeField] private float maxTime;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private float fadeTime;
    
    /// <param name="bool">True means victory, false means defeat</param>
    public Action<bool> OnGameOver = delegate { };
    public Action OnGameRestart = delegate { };
    
    public static bool IsPaused { get; private set; }
    public static bool IsGameEnding { get; set; }
    public static GameManager Instance { get; private set; }
    
    public float TimeRemaining { get; private set; }

    private Dictionary<GameObject, Image> _backgroundImageDict = new();

    private void Awake() => Instance = this;
    public void RestartGame() => OnGameRestart?.Invoke();

    private void Start()
    {
        OnGameOver += GameOver;
        OnGameRestart += Restart;
        
        Restart(); //Reusing the function for the first time the game starts.
        
        _backgroundImageDict.Add(gameWonPanel, gameWonPanel.GetComponent<Image>());
        _backgroundImageDict.Add(gameLostPanel, gameLostPanel.GetComponent<Image>());
    }

    private void Update()
    {
        if (IsPaused) return;
        TimeRemaining -= Time.deltaTime;
        timerText.text = TimeRemaining.ToString("0.0");
        if (TimeRemaining <= 0)
            //True means victory
            OnGameOver?.Invoke(true);
    }

    private void Restart()
    {
        TimeRemaining = maxTime;
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
        StartCoroutine(GameOverRoutine(win ? _backgroundImageDict[gameWonPanel] : _backgroundImageDict[gameLostPanel]));
        buttonsPanel.SetActive(false);
    }

    /// <summary>
    /// Fade effect, fades in the chosen image
    /// </summary>
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
    }
}
