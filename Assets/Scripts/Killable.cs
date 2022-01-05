using UnityEngine;
using UnityEngine.UI;

public class Killable : MonoBehaviour
{

    public int lives;
    private const int maxLives = 3;
    private GameObject human;

    void Start()
    {
        this.lives = maxLives;
        this.human = this.gameObject; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullets"))
            this.DecreaseLife();
    }

    void DecreaseLife()
    {
        this.human.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, (1.0f / (float)maxLives) * (float)this.lives);
        this.lives--;
        if (this.lives <= 0)
        {
            SoundManagerScript.PlaySound("death");
            ScoreScript.AddScore();
            Destroy(gameObject);
            return;
        }
    }
}
