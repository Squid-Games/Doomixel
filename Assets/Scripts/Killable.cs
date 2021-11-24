using UnityEngine;

public class Killable : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullets")
            Destroy(gameObject);
    }
}
