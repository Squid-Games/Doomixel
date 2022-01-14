using UnityEngine;
using UnityEngine.AI;
using System;

public class Killable : MonoBehaviour
{

    public int lives;
    private int maxLives;
    private GameObject human;


    private bool _isDead = false;

    public bool IsDead() => _isDead;

    void Start()
    {
        if((int)Settings.GetDifficulty()==1)
            lives = 3;
        else if((int)Settings.GetDifficulty()==2)
            lives = 4;
        else if((int)Settings.GetDifficulty()==3)
            lives = 5;
        maxLives = lives;
        human = this.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isDead)
            return;

        if (other.gameObject.CompareTag("Bullets"))
        {
            int aux = 0;

            Int32.TryParse(other.gameObject.GetComponent<MeshRenderer>().material.name.Split()[0].Split('_')[1], out aux);
          
            Decrease(aux, this.name.Split('(')[0]);
            if(aux!=1)
                Destroy(other.gameObject);
        }

    }

    private void Update()
    {
        human.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1.0f / maxLives * lives);
    }

    void Decrease(int bulletIndex, string enemyName)
    {
        if (bulletIndex == 0)
            DecreaseLife(1);
        else if (bulletIndex == 1)
            DecreaseLife(5);
        else if (bulletIndex == 2 && enemyName.Equals("Human_1"))
            DecreaseLife(6);
        else if (bulletIndex == 3)
            DecreaseLife(2);
        else if (bulletIndex == 4)
        {
            gameObject.GetComponent<NavMeshAgent>().speed=2f;
            gameObject.GetComponent<NavMeshAgent>().acceleration = 6f;
        }
        else if (bulletIndex == 5 && enemyName.Equals("Human_2"))
            DecreaseLife(4);
        else if (bulletIndex == 6)
            DecreaseLife(1);//fire

        else
            DecreaseLife(1);
    }


    void DecreaseLife(int number)
    {
        lives = lives - number;
        if (lives <= 0)
            Kill();
    }

    public void Kill()
    {
        SoundManagerScript.PlaySound("death");
        Control.Reward(UnityEngine.Random.Range(1, 7));
        _isDead = true;
        ScoreScript.AddScore(10);

    }
}