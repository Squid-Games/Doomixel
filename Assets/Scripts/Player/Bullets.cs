using UnityEngine;

public class Bullets : MonoBehaviour
{
    private const float DISTANCE_TO_DESPAWN = 100.0f;

    public Vector3 velocity = Vector3.zero;

    private float _distanceInAir;

    void OnTriggerEnter(Collider other)
    {
        if(gameObject.CompareTag("BulletsEnemy") && (other.gameObject.CompareTag("Human") || other.gameObject.CompareTag("Enemy")))
            return;

        if (!other.gameObject.CompareTag("Bullets") && !other.gameObject.CompareTag("RewardAmmo") && !other.gameObject.CompareTag("BulletsEnemy"))
            Destroy(gameObject);

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
