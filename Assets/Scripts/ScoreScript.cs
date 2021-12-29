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
    private static int resetTimer = 0;
    public Text txt;

    void Start()
    {
        this._playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (this._playerController.StartTimer() == 1)
        {
            timer += Time.deltaTime;
            if (resetTimer == 1)
            {
                score = score + ((1 / timer) * 100);
                score = Mathf.RoundToInt(score);
                txt.text = score.ToString();
                timer = 0;
                resetTimer = 0;
            }
        }
    }

    public static void AddScore()
    {
        resetTimer = 1; 
    }
}
