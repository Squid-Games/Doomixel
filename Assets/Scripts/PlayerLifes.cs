using UnityEngine;
using UnityEngine.UI;

public class PlayerLifes : MonoBehaviour
{
    public Image[] hearts;
    public int life;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < life;
        }
    }

    void UpdatePlayerLife(int life)
    {
        this.life = life;
    }
    
    void DecreasePlayerLife(int life)
    {
        this.life--;
    }
}