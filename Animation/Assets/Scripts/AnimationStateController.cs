using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator _animator;

    [Header ("Speed")]
    //float _velocityZ = 0f;
    //float _velocityX = 0f;
    [SerializeField] float _acceleration = 1f;
    [SerializeField] float _deceleration = 1f;
    [SerializeField] float _maxWalkSpeed = 1f;
    [SerializeField] float _maxRunSpeed = 1f;

    [Header("Jumping")]
    Rigidbody _rb;
    bool _isGrounded;
    [SerializeField] float _jumpForce = 3f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float _roundCheckRadius = 0.3f;
    [SerializeField] LayerMask _groundLayer;

    Vector2 _movementInput;
    Vector3 _velocity = new Vector3();
    bool _isSprinting;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        _rb.useGravity = true;       //I think this can also be done in editor
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        InputManager.Instance.MoveEvent += (Vector2 input) => _movementInput = input;
        InputManager.Instance.JumpEvent += TryJump;
        InputManager.Instance.AttackEvent += TryAttack;
        InputManager.Instance.SprintStartEvent += () => _isSprinting = true;
        InputManager.Instance.SprintReleasedEvent += () => _isSprinting = false;
    }


    private void FixedUpdate()
    {   // -- Ground Check -- 


        // -- Input -- 
        float currentMaxSpeed = _isSprinting ? _maxRunSpeed : _maxWalkSpeed;
        _velocity.z += (_movementInput.y == 0) 
            ? Mathf.MoveTowards(_velocity.z, 0f, _deceleration * Time.deltaTime) 
            : _movementInput.y * Time.deltaTime * _acceleration;

        _velocity.x += (_movementInput.x == 0)
            ? Mathf.MoveTowards(_velocity.x, 0f, _deceleration * Time.deltaTime)
            : _movementInput.x * Time.deltaTime * _acceleration;


        _velocity.z = Mathf.Clamp(_velocity.z, -currentMaxSpeed, currentMaxSpeed);
        _velocity.x = Mathf.Clamp(_velocity.x, -currentMaxSpeed, currentMaxSpeed);

        _animator.SetFloat("VelocityZ", _velocity.z);
        _animator.SetFloat("VelocityX", _velocity.x);


        //Apply movement to Rigidbody
        _velocity.y = _rb.linearVelocity.y;
        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, _velocity, 10f * Time.deltaTime);

        Debug.Log(_velocity);
        
        // -- Ground Movement -- 
    }

    public void TryJump()
    {

    }

    public void TryAttack()
    {

    }

}
