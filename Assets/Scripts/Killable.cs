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
        this.human = GameObject.FindGameObjectWithTag("Human");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullets")
            this.DecreaseLife();
    }

    void DecreaseLife()
    {
        this.human.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, (1.0f / (float)maxLives) * (float)this.lives);
        this.lives--;
        if (this.lives <= 0)
        {
            SoundManagerScript.PlaySound("death");
            Destroy(gameObject);
            return;
        }
    }
}
