using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MIN_MOVEMENT_BIAS = 0.01f;

    public float mouseSensivity = 300.0f;
    public float movementSpeed = 10.0f;
    public float timeToShootBullet = 0.1f;
    public float bulletSpeed = 25.0f;

    public GameObject bulletsPrefab;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private float _accumulatedShootTime;

    public float lives = 5;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        Cursor.lockState = CursorLockMode.Locked;
        _accumulatedShootTime = 0.0f;
    }

    void Update()
    {
        var diffMouseX = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0.0f, diffMouseX * mouseSensivity, 0.0f) * Time.deltaTime);

        if (Input.GetButton("Fire1"))
        {
            _accumulatedShootTime += Time.deltaTime;

            while (_accumulatedShootTime >= timeToShootBullet)
            {
                SpawnBullet();
                _accumulatedShootTime -= timeToShootBullet;
            }
        }
    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var inputVector = new Vector3(horizontal, 0.0f, vertical);

        _rigidbody.angularVelocity = Vector3.zero;

        if (inputVector.magnitude < MIN_MOVEMENT_BIAS)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        
        var rotatedVector = GetCurrentAngleAxis() * inputVector;

        _rigidbody.velocity = rotatedVector * movementSpeed;

    }

    void SpawnBullet()
    {
        if (bulletsPrefab is null)
            return;

        var bullets = Instantiate(bulletsPrefab, transform.position, Quaternion.identity);
        Physics.IgnoreCollision(bullets.GetComponent<Collider>(), _collider);
        bullets.GetComponent<Bullets>().velocity = GetCurrentAngleAxis() * Vector3.forward * bulletSpeed;
    }

    private Quaternion GetCurrentAngleAxis()
    {
        return Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
    }

	void DecreaseLife()
	{
		this.lives -= 1;
		if(this.lives<=0){
			/// TODO: Game over
		}
			
	}
}
