using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<ScoreObject> values;

    public ScoresObject()
    {
        try {
            values = FileHandler.ReadFromJSON<List<ScoreObject>>("ScoreHistory.json");
        }
        catch (Exception) {
            values = new List<ScoreObject>();
        }
    }
    
    public void New(DateTime time, int value = 0)
    {
        values.Add(new ScoreObject(time, value));
    }

    public void Add(int value)
    {
        if(values.Count==0)
        {
            New(DateTime.Now, value);
            return;
        }
        values[values.Count - 1].value += value;
    }

    public int Get()
    {
        if(values.Count>0)
        {
            return values.Last().value;
        }

        return 0;
    }

    public void Save()
    {
        FileHandler.SaveToJSON(values, "ScoreHistory.json");
    }
}

public class ScoreScript : MonoBehaviour
{
    private float realTimer = 0.0f;
    // Start is called before the first frame update
    private PlayerController _playerController;
    public Text scoreText;
    public Text timerText;
    private float _displayScore;
    private float _transitionSpeed = 100;
    public static ScoresObject scores;

    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        realTimer += Time.deltaTime;
        _displayScore = Mathf.MoveTowards(_displayScore, scores.Get(), _transitionSpeed * Time.deltaTime);
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

    private void LoadScores()
    {
        scores = FileHandler.ReadFromJSON<ScoresObject>("Scores.json");
        Debug.Log(scores);
    }
    
    public static void ResetScore()
    {
        scores.New(DateTime.Now);
    }

    public static void AddScore(int points)
    {
        if (scores == null)
        {
            scores = new ScoresObject();
        }
        scores.Add(points);
        SaveScore();
    }

    public static void SaveScore()
    {
        scores.Save();
    }
}
