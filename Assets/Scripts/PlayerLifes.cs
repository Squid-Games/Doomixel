using UnityEngine;
using UnityEngine.UI;

public class PlayerLifes : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

	private PlayerController _playerController;
	
	void Start()
	{
		this._playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < this._playerController.lives;
        }
    }
}