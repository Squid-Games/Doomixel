using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public Text TextModel;
    public Text HighScoreText;
    public GameObject ScrollContent;

    public void Start()
    {
        GenerateLeaderboard();
    }

    void GenerateLeaderboard()
    {
        
        int scoresLength = MainMenu.savedData.scoresHistory.Count;
        int maxScore = 0;
        for (int i=0;i<scoresLength;i++)
        {
            int score = MainMenu.savedData.scoresHistory[scoresLength - i - 1];
            
            Text duplicate = Instantiate(TextModel, TextModel.transform.parent);
            duplicate.transform.position += new Vector3(0, -70 * (i+1), 0);
            duplicate.text = $"  {scoresLength - i}              " + string.Format("{0:000000}", score);
            duplicate.fontStyle = FontStyle.Normal;
            if (score > maxScore)
                maxScore = score;
            
            ScrollContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 100);
        }
        HighScoreText.text = string.Format("{0:000000}", maxScore);
    }
}