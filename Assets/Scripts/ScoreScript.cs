using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    private float timer = 0.0f;
    // Start is called before the first frame update
    private float score;
    private PlayerController _playerController;
    private static int _resetTimer = 0;
    public Text scoreText;
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
        
        _displayScore = Mathf.MoveTowards(_displayScore, score, _transitionSpeed * Time.deltaTime);
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        Debug.Log(_displayScore);
        scoreText.text = string.Format("Score: {0:000000}", _displayScore);
    }

    public static void AddScore()
    {
        _resetTimer = 1; 
    }

    public void SaveScore()
    {
        
    }
}
