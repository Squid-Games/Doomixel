using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public Text TextModel;
    public GameObject ScrollContent;
    private int generatedScores = 0;
    
    public void Start()
    {
        GenerateLeaderboard();
    }

    public void Update()
    {
        if(ScoreScript.scoresHistory.values.Count > generatedScores)
            GenerateLeaderboard();
    }

    void GenerateLeaderboard()
    {
        generatedScores = ScoreScript.scoresHistory.values.Count;
        for (int i=0;i<ScoreScript.scoresHistory.values.Count;i++)
        {
            ScoreObject item = ScoreScript.scoresHistory.values[i];
            
            Text duplicate = Instantiate(TextModel, TextModel.transform.parent);
            duplicate.transform.position += new Vector3(0, -70 * (i+1), 0);
            duplicate.text = $"#{i+1} #{item.value}";
            duplicate.fontStyle = FontStyle.Normal;
            
            ScrollContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 130);
        }
    }
}