using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreObject
{
    public DateTime time;
    public int value;

    public ScoreObject(DateTime time, int value)
    {
        this.time = time;
        this.value = value;
    }
}

public class ScoresObject
{
    public List<ScoreObject> values;

    public ScoresObject()
    {
        values = new List<ScoreObject>();
        if (FileHandler.Exists("ScoresHistory.json"))
            values = FileHandler.ReadFromJSON<List<ScoreObject>>("ScoresHistory.json");
    }
    
    public void New(DateTime time, int value = 0)
    {
        values.Add(new ScoreObject(time, value));
    }

    public void Save()
    {
        FileHandler.SaveToJSON(values, "ScoresHistory.json");
    }
}

public class ScoreScript : MonoBehaviour
{
    private float realTimer = 0.0f;
    private PlayerController _playerController;
    public Text scoreText;
    public Text timerText;
    private float _displayScore;
    private float _transitionSpeed = 100;
    public static ScoresObject scoresHistory;
    private static int _score = 0;

    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        realTimer += Time.deltaTime;
        _displayScore = Mathf.MoveTowards(_displayScore, _score, _transitionSpeed * Time.deltaTime);
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

    public static void AddScore(int points)
    {
        _score += points;
    }

    public static void SaveScore()
    {
        scoresHistory.New(DateTime.Now, _score);
        scoresHistory.Save();
        _score = 0;
    }
}
