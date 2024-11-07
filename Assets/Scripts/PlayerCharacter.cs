using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _speedWalk;
    [SerializeField] private float _speedRotate;
    [SerializeField] private float _speedRun;
    [SerializeField] private float _speedSneak;
    [SerializeField] private float _forceJamp;
    [SerializeField] private Transform _pointGraundCheker;

    private PlayerControl _control;
  //  private AudioManager _audioManager;
    private Rigidbody _rigidbody;
    private float _currentSpeed;
    private float _walkColliderHeight;
    private float _sneakColliderSize = 1f;
    private Vector3 _sneakColliderCenter = new Vector3(0, -0.5f, 0);
    private Vector3 _camerPocitionSneak = new Vector3(0, 0, -0.5f);
    private Vector3 _camerPositionWalk = new Vector3(0,0.6f,-0.5f);
    private CapsuleCollider _collider;
    private bool _isSneaking;

    private void Awake()
    {
        _control = new PlayerControl();
        SetCursorSetting();
        //_audioManager = GetComponent<AudioManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentSpeed = _speedWalk;
        _collider = GetComponent<CapsuleCollider>();
        _walkColliderHeight = _collider.height;
        _control.Player.Jamp.started += context => Jamp();
        // Подпись на кнопки бега и приседа
        _control.Player.Run.started += context => Run();
        _control.Player.Sneak.started += context => Sneak();
        // Подпись на возвращение к хотьбе
        _control.Player.Run.canceled += context => Walk();
        _control.Player.Sneak.canceled += context => Walk();
    }

    /// <summary>
    /// Для плавного поворота
    /// </summary>
    private void Update()
    {
        Rotate(_control.Player.Look.ReadValue<Vector2>());        
    }

    /// <summary>
    /// Для корректного расчета физики при передвижении 
    /// </summary>
    private void FixedUpdate()
    {
        Move(_control.Player.Move.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        _control.Enable();
    }

    private void OnDisable()
    {
        _control.Disable();
    }

    private void Run() 
    {
        if (IsOnTheGraund())
            _currentSpeed = _speedRun;
    //    _audioManager.PlayAudioByNum(1);
    }

    private void Walk() 
    { 
        _currentSpeed = _speedWalk;
    //    _audioManager.PlayAudioByNum(0);
        if (_isSneaking)
        {
            _collider.height = _walkColliderHeight;
            _collider.center = Vector3.zero;
            _cameraTransform.localPosition = _camerPositionWalk;
            _isSneaking = false;
        }
    }

    private void Sneak() 
    {
        _currentSpeed = _speedSneak;
        _collider.height = _sneakColliderSize;
        _collider.center = _sneakColliderCenter;
        _cameraTransform.localPosition = _camerPocitionSneak;
        _isSneaking = true;
     //   _audioManager.StopAudio();
    }

    private bool IsOnTheGraund() 
    {
        Collider[] colliders = Physics.OverlapSphere(_pointGraundCheker.position, 0.1f);
        Debug.Log(colliders.Length);
        foreach (Collider collider in colliders)
        {
            if (collider.tag != "Rooms" && collider.tag != "Player") return true;
        }
        return false;
    }

    private void Jamp() 
    {
        
        if (IsOnTheGraund())
        {
            Vector3 jampForse = Vector3.zero;
            jampForse.y = _forceJamp;
            _rigidbody.AddForce(jampForse);
        }
        
    }

    private void Move(Vector2 direction) 
    {
        //   if (direction == Vector2.zero || _isSneaking) _audioManager.StopAudio();
        //    else _audioManager.PalyAudioIfNotPlaying();
        Vector3 velosity = (transform.forward * direction.y + transform.right * direction.x) * _currentSpeed;
        velosity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velosity;
    }

    private void Rotate(Vector2 rotate)
    {
        float mauseX = rotate.x * _speedRotate * Time.deltaTime;
        float mauseY = rotate.y * _speedRotate * Time.deltaTime;

        float rotateX = Mathf.Clamp(-mauseY, -90, 90);

        Quaternion rotation = _cameraTransform.localRotation;       
        rotation *= Quaternion.Euler(rotateX, 0, 0);
        if (rotation.x < 0.55 && rotation.x > -0.55)
            _cameraTransform.localRotation = rotation;
        // Разрешаем поповрот только по мыши. Исключаем поворот при столкновении
        _rigidbody.freezeRotation = false;
        transform.Rotate(0, mauseX, 0);
        _rigidbody.freezeRotation = true;
    }

    public void SetCursorSetting() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DiactivateControl() 
    {
        _control.Disable();
    }

    public void ActivateControl() 
    {
        _control.Enable();
    }   
}
