using UnityEngine;
using TMPro;

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

    public int boolStart = 0;
    public GameObject weapon;
    private Vector3 weapon_origin;
    private float weapon_pos = 0.0f;
    private const float MAX_SWING_VAL = 40.0f;
    private int swing_orientation = -1;
    private float swing_speed = 150.0f;
    
    private float move_delay = 0.0f; 

    float calculateMovePath(float x) {
        return 0.0025f * Mathf.Pow(x, 2);
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        weapon_origin = weapon.transform.position;

        Cursor.lockState = CursorLockMode.Locked;
        _accumulatedShootTime = 0.0f;
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.None)
            return;
        
        var diffMouseX = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0.0f, diffMouseX * mouseSensivity, 0.0f) * Time.deltaTime);

        if (Input.GetButton("Fire1"))
        {
            weapon.transform.position = weapon_origin;
            move_delay = 0.25f;
            weapon_pos = 0.0f;
            boolStart = 1;

            _accumulatedShootTime += Time.deltaTime;

            while (_accumulatedShootTime >= timeToShootBullet)
            {
                SpawnBullet();
               
                _accumulatedShootTime -= timeToShootBullet;
            }
        }

    }

    void MoveWeapon() {
        float new_pos_y = calculateMovePath(weapon_pos);
        var posVector = new Vector3(weapon_pos, new_pos_y, 0.0f);

        weapon_pos += swing_speed * swing_orientation * Time.deltaTime;
        if(Mathf.Abs(weapon_pos) > MAX_SWING_VAL) {
            weapon_pos = MAX_SWING_VAL * swing_orientation;
            swing_orientation *= -1;
        }

        weapon.transform.position = weapon_origin + posVector;
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
        if(move_delay == 0)
        {
            MoveWeapon();
        }
        else
        {
            move_delay = Mathf.Max(0.0f, move_delay - Time.deltaTime);
        }
        
    }

    void SpawnBullet()
    {
        if (bulletsPrefab is null)
            return;

        if (Control.selected_bullet is null)
        {
            SoundManagerScript.PlaySound("gunshot_empty");
            return;
        }

        var bullets = Instantiate(bulletsPrefab, transform.position, Quaternion.identity);
        Physics.IgnoreCollision(bullets.GetComponent<Collider>(), _collider);
        bullets.GetComponent<Bullets>().velocity = GetCurrentAngleAxis() * Vector3.forward * bulletSpeed;


        if (Control.selected_bullet != null)
        {

            if (Control.selected_bullet.GetAmmo() >= 1)
            {
                SoundManagerScript.PlaySound("gunshot");

                if (Control.selected_bullet.id != 0)
                {  
                    Control.selected_bullet.ammo -= 1;
                    Control.selected_border.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = Control.selected_bullet.GetAmmo().ToString("0");
                }
                else
                    Control.selected_border.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "\u221E";
               
                bullets.GetComponent<MeshRenderer>().material = Control.selected_bullet.GetMaterial();
            }
            else 
            { 
                bullets.GetComponent<MeshRenderer>().material = null;
            }
        }
        else 
        { 
            bullets.GetComponent<MeshRenderer>().material = null;
        }
    }

    private Quaternion GetCurrentAngleAxis()
    {
        return Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
    }

    public void DecreaseLife()
    {
        lives -= 1;
        if (lives <= 0)
        {
            // TODO: Game over
        }

    }
}
