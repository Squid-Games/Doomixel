using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    private float timer = 0.0f;
    // Start is called before the first frame update
    private float score = 0;
    private PlayerController _playerController;
    private static int _resetTimer = 0;
    public Text scoreText;

    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.StartTimer() == 1)
        {
            timer += Time.deltaTime;
            if (_resetTimer == 1)
            {
                score = score + ((1 / timer) * 100);
                score = Mathf.RoundToInt(score);
                scoreText.text = "Score: " + score;
                timer = 0;
                _resetTimer = 0;
            }
        }
    }

    public static void AddScore()
    {
        _resetTimer = 1; 
    }
}
