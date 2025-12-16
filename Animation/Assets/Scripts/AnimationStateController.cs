using System;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator _animator;

    [Header ("Speed")]
    [SerializeField] float _acceleration = 2.5f;
    [SerializeField] float _deceleration = 2.5f;
    [SerializeField] float _maxRunSpeed = 5f;

    [Header("Jumping")]
    Rigidbody _rb;
    bool _isGrounded;
    [SerializeField] float _jumpForce = 3f;
    [SerializeField] Transform _groundCheckStart;
    [SerializeField] float _groundCheckDistance = 0.025f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _airborneMovementMultiplier = 0.25f;

    [Header("Looking")]
    [SerializeField] float _lookSensitivity = 1f;

    Vector2 _movementInput;
    Vector3 _velocity = new Vector3();
    bool _isSprinting;
    float _currentMaxSpeed;

    private void Start()
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
        
        InputManager.Instance.LookEvent += ProcessLook;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void FixedUpdate()
    {   // -- Ground Check -- 
        _isGrounded = Physics.Raycast(_groundCheckStart.position, Vector3.down, _groundCheckDistance, _groundLayer);
        _animator.SetBool("IsGrounded", _isGrounded);

        // -- Input -- 
        _currentMaxSpeed = _isSprinting ? _maxRunSpeed : _maxRunSpeed/2f;
        _velocity.z = (_movementInput.y == 0) 
            ? Mathf.MoveTowards(_velocity.z, 0f, _deceleration * Time.fixedDeltaTime)   //decelerate
            : _velocity.z + (_movementInput.y * Time.fixedDeltaTime * _acceleration);   //accelerate v = v + a*t

        _velocity.x = (_movementInput.x == 0)
            ? Mathf.MoveTowards(_velocity.x, 0f, _deceleration * Time.fixedDeltaTime)
            : _velocity.x + (_movementInput.x * Time.fixedDeltaTime * _acceleration);


        _velocity.z = Mathf.Clamp(_velocity.z, -_currentMaxSpeed, _currentMaxSpeed);
        _velocity.x = Mathf.Clamp(_velocity.x, -_currentMaxSpeed, _currentMaxSpeed);

        if (_isGrounded)
        {
            // -- Ground Movement -- 
            _animator.SetFloat("VelocityZ", _velocity.z / _maxRunSpeed);    //has to be normalized btwn 0 and 1 for the animator to be accurate
            _animator.SetFloat("VelocityX", _velocity.x / _maxRunSpeed);

            _velocity.y = _rb.linearVelocity.y;
            //_rb.linearVelocity = _velocity;     //Vector3.Lerp(_rb.linearVelocity, _velocity, 10f * Time.fixedDeltaTime);
            _rb.AddForce(transform.forward * _velocity.z);
        } 
        else        // -- Air Movement -- 
        {
            _animator.SetFloat("VelocityY", _rb.linearVelocity.y);
        }
        
    }

    public void TryJump()
    {
        if(_isGrounded == false) return;

        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _animator.SetTrigger("JumpTakeoff");
    }

    public void TryAttack()
    {
        if(_isGrounded)
            _animator.SetTrigger("Attack");
    }

    private void ProcessLook(Vector2 lookDelta)
    {
        //pitch
        
        //yaw
        transform.Rotate(0f, lookDelta.x * _lookSensitivity, 0f);
    }

}
