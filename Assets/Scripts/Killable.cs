using UnityEngine;
using UnityEngine.UI;

public class Killable : MonoBehaviour
{

	public int life;
	public Sprite fullHeart;
    public Sprite emptyHeart;
	public Image[] hearts;

    void Start()
    {
		
    }

    void Update()
    {
		for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < life;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullets"){
			this.life--;
			if(this.life<=0)
				Destroy(gameObject);
		}
    }
}
