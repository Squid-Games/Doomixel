using UnityEngine;
using UnityEngine.UI;

public class Killable : MonoBehaviour
{

    public int lives;
    private const int maxLives = 3;
    private GameObject human;

    private bool _isDead = false;

    public void Kill() => _isDead = true;
    public bool IsDead() => _isDead;

    void Start()
    {
        this.lives = maxLives;
        this.human = this.gameObject; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isDead)
            return;

        if (other.gameObject.CompareTag("Bullets"))
        { 
            this.DecreaseLife();
            Destroy(other.gameObject);
        }
            
    }

    void DecreaseLife()
    {
        this.human.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, (1.0f / (float)maxLives) * (float)this.lives);
        this.lives--;
        if (this.lives <= 0)
        {
            SoundManagerScript.PlaySound("death");
            ScoreScript.AddScore();
            Control.reward(Random.Range(0, 7));
            _isDead = true;
            return;
        }
    }
}
