using UnityEngine;

public class Bullets : MonoBehaviour
{
    private const float DISTANCE_TO_DESPAWN = 100.0f;

    public Vector3 velocity = Vector3.zero;

    private float _distanceInAir;

    void OnCollisionExit(Collision other)
    {
        //trebuie luat dupa tag sau nume 
        Debug.Log("hello");

    }
    void Start()
    {
        _distanceInAir = 0.0f;
    }

    void Update()
    {
        var movement = velocity * Time.deltaTime;
        transform.localPosition += movement;
        _distanceInAir += movement.magnitude;

        if (_distanceInAir >= DISTANCE_TO_DESPAWN)
            Destroy(gameObject);
    }
}
