using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float jumpSpeed = 5;
    public float mouseSensitivity = 4.0f;
    public float pitchRange = 170.0f;

    public Weapon[] weapons;    // weapon "inventory"
    public int weaponIndex = 0; // current selected weapon

    private float verticalRotation = 0f;
    private float verticalSpeed = 0f;

    private CharacterController characterController;
    private Rigidbody rb;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ControlRotation();
        ControlTranslation();
        HandleGravity();
        ControlWeapons();
    }

    void ControlRotation()
    {
        float rotYaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotYaw, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -pitchRange, pitchRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Rotate current weapon to match
        weapons[weaponIndex].transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void ControlTranslation()
    {
        Vector3 velocity = new Vector3(0f, 0f, 0f);
        // Harvest input
        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;


        if (Input.GetButtonDown("Jump") && characterController.isGrounded) // Only return true if the button has been pressed since the last frame, not if it's been held down AND if characterController is on the ground
        {
            verticalSpeed = jumpSpeed;
        }

        // Gravity
        verticalSpeed += Physics.gravity.y * Time.deltaTime;


        velocity = new Vector3(sideSpeed, verticalSpeed, forwardSpeed); // x, y, z

        // Apply translation
        velocity = transform.rotation * velocity;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleGravity()
    {

    }

    void ControlWeapons()
    {
        if(Input.GetMouseButtonDown(0))
        {
            weapons[weaponIndex].Fire(rb.velocity);
        }
    }
}