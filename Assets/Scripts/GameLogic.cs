using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject playerObject;

    void Start()
    {
        AssignPlayer();
    }

    void Update()
    {
        if (playerObject is null)
            AssignPlayer();
    }

    private void AssignPlayer()
    {
        playerObject = GameObject.FindWithTag("Player");
    }
}
