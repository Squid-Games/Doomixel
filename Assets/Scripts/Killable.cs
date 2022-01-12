using UnityEngine;
using UnityEngine.UI;

public class Killable : MonoBehaviour
{

    public int lives;
    private const int maxLives = 3;
    private GameObject human;

    private bool _isDead = false;

    public bool IsDead() => _isDead;

    void Start()
    {
        lives = maxLives;
        human = this.gameObject; 
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

    private void Update()
    {
        human.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, (1.0f / (float)maxLives) * (float)(this.lives));
    }

    void DecreaseLife()
    {
        lives--;
        if (lives <= 0)
            Kill();
    }

    public void Kill()
    {
        SoundManagerScript.PlaySound("death");
        ScoreScript.AddScore(10);
        Control.reward(Random.Range(0, 7));
        _isDead = true;
    }
}
