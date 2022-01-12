using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public GameObject TextModel;
    public void Start()
    {
        GenerateLeaderboard();
    }

    void GenerateLeaderboard()
    {
        for (int i = 1; i <= 10; i++)
        {
            GameObject duplicate = Instantiate(TextModel, TextModel.transform.parent);
            duplicate.transform.position += new Vector3(0, -70 * i, 0);
            // duplicate.GetComponent<Text>().text = $"#{i} 100";
            // duplicate.GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
    }
}