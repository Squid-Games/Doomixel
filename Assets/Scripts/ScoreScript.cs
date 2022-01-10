using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    private float timer = 0.0f;
    private float realTimer = 0.0f;
    // Start is called before the first frame update
    private float score;
    private PlayerController _playerController;
    private static int _resetTimer = 0;
    public Text scoreText;
    public Text timerText;
    private float _displayScore;
    private float _transitionSpeed = 100;

    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.StartTimer() == 1)
        {
            timer += Time.deltaTime;
            if (_resetTimer == 1)
            {
                score += ((1 / timer) * 100);
                score = Mathf.RoundToInt(score);
                timer = 0;
                _resetTimer = 0;
            }
        }
        
        realTimer += Time.deltaTime;
        _displayScore = Mathf.MoveTowards(_displayScore, score, _transitionSpeed * Time.deltaTime);
        UpdateScoreDisplay();
        UpdateTimeDisplay();
    }

    void UpdateScoreDisplay()
    {
        scoreText.text = string.Format("Score: {0:000000}", _displayScore);
    }

    void UpdateTimeDisplay()
    {
        string minutes = Math.Floor(realTimer / 60).ToString("00");
        string seconds = (realTimer % 60).ToString("00");
     
        timerText.text = string.Format("Time: {0}:{1}", minutes, seconds);
    }
    
    public static void AddScore()
    {
        _resetTimer = 1; 
    }

    public void SaveScore()
    {
        
    }
}
