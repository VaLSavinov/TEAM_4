using System;
using System.Collections;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 18f; // Базовая скорость
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private const float GRAVITY = -10f;
    private const float GROUNDED_VELOCITY = -1f;
    private bool isGrounded;
    private Vector3 velocity;
    private PlayerControl _control;
    private float _currentSpeed;

    private float originalHeight;
    private bool _isAlive = true;

    void Awake()
    {
        originalHeight = controller.height;
        _control = new PlayerControl();
        Walk();
        // Подпись на кнопки бега и приседа
        _control.Player.Run.started += context => Run();
        _control.Player.Sneak.started += context => Sneak();
        // Подпись на возвращение к хотьбе
        _control.Player.Run.canceled += context => Walk();
        _control.Player.Sneak.canceled += context => Walk();
        GameMode.FirstPersonMovement = this;
    }
    private void OnEnable()
    {
        _control.Enable();
    }

    private void OnDisable()
    {
        _control.Disable();
    }

    private void Sneak()
    {
        _currentSpeed = speed / 1.5f; // Приседание
        controller.height = originalHeight / 2;
    }

    private void Walk()
    {
        _currentSpeed = speed;
        controller.height = originalHeight;
    }

    private void Run()
    {
        _currentSpeed =  speed * 1.5f; // Спринт
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        GameMode.PlayerUI.GameOver();
        
    }

    void Update()
    {
        Vector3 moveDirection = GetMoveDirection();
        HandleNormalMovement(moveDirection);
    }
    
    Vector3 GetMoveDirection()
    {
        float x = InputManager.Instance.Horizontal;
        float z = InputManager.Instance.Vertical;
        float y = 0;


        Vector3 move = transform.right * x + transform.up * y + transform.forward * z;
        return move * _currentSpeed * Time.deltaTime;
    }

    void HandleNormalMovement(Vector3 moveDirection)
    {
        if (!_isAlive) return;
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

    public void Die() 
    {
        _isAlive = false;
        _control.Disable();
        StartCoroutine(GameOver());
    }

    public bool IsAlive() => _isAlive;
}