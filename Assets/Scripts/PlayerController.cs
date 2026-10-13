using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Transform flashlight;

    [Header("Mouvement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Lampe torche")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minPitch = -70f;
    [SerializeField] private float maxPitch = 70f;

    private CharacterController controller;
    private float verticalVelocity;

    private float flashlightYaw;
    private float flashlightPitch;
    private Quaternion flashlightInitialRotation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (flashlight != null)
        {
            flashlightInitialRotation = flashlight.localRotation;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RotatePlayer();
        MovePlayer();
        RotateFlashlight();
    }

    private void RotatePlayer()
    {
        if (Keyboard.current == null)
            return;

        float rotation = 0f;

        if (Keyboard.current.qKey.isPressed ||
            Keyboard.current.aKey.isPressed)
        {
            rotation -= 1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            rotation += 1f;
        }

        transform.Rotate(Vector3.up, rotation * rotationSpeed * Time.deltaTime,Space.World);
    }

    private void MovePlayer()
    {
        if (Keyboard.current == null)
            return;

        float forwardInput = 0f;

        if (Keyboard.current.wKey.isPressed)
        {
            forwardInput += 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            forwardInput -= 1f;
        }

        Vector3 direction = transform.forward;
        direction.y = 0f;
        direction.Normalize();

        Vector3 movement = direction * forwardInput * moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;

        controller.Move(movement * Time.deltaTime);
    }

    private void RotateFlashlight()
    {
        if (flashlight == null || Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        flashlightYaw += mouseDelta.x * mouseSensitivity;
        flashlightPitch -= mouseDelta.y * mouseSensitivity;

        flashlightPitch = Mathf.Clamp(flashlightPitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(flashlightPitch, flashlightYaw, 0f);

        flashlight.localRotation = flashlightInitialRotation * rotation;
    }
}
