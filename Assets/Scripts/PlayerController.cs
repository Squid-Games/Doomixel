using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MIN_MOVEMENT_BIAS = 0.01f;

    public float mouseSensivity = 300.0f;
    public float movementSpeed = 10.0f;

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        var diffMouseX = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0.0f, diffMouseX * mouseSensivity, 0.0f) * Time.deltaTime);
    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var inputVector = new Vector3(horizontal, 0.0f, vertical);
        if (inputVector.magnitude < MIN_MOVEMENT_BIAS)
            return;

        var rotatedVector = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * inputVector;

        _rigidbody.velocity = rotatedVector * movementSpeed;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
