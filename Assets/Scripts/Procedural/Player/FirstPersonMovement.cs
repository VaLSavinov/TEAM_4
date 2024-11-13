using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 18f; // Базовая скорость
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private const float GRAVITY = -50f;
    private const float GROUNDED_VELOCITY = -2f;
    private bool isGrounded;
    private Vector3 velocity;

    private float originalHeight;

    void Start()
    {
        originalHeight = controller.height;
    }

    void Update()
    {
        float currentSpeed = CalculateSpeed();
        Vector3 moveDirection = GetMoveDirection(currentSpeed);
        HandleNormalMovement(moveDirection);
    }

    float CalculateSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return speed * 1.5f; // Спринт
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            return speed / 1.5f; // Приседание
        }
        else
        {
            return speed; // Обычная ходьба
        }
    }

    Vector3 GetMoveDirection(float currentSpeed)
    {
        float x = InputManager.Instance.Horizontal;
        float z = InputManager.Instance.Vertical;
        float y = 0;


        Vector3 move = transform.right * x + transform.up * y + transform.forward * z;
        return move * currentSpeed * Time.deltaTime;
    }

    void HandleNormalMovement(Vector3 moveDirection)
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = GROUNDED_VELOCITY;
        }

        controller.Move(moveDirection);

        if (InputManager.Instance.JumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
        }

        velocity.y += GRAVITY * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}