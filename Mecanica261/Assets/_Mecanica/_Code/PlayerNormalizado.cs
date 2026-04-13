using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerNormalizado : MonoBehaviour
{
    [Header("Movement")]
    public float _playerSpeed = 2.0f;
    public float _jumpHeight = 1.5f;
    public float _gravityValue = -9.81f;

    public CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;

    [Header("Input Actions")]
    public InputActionReference _moveAction;
    public InputActionReference _jumpAction;

    private void OnEnable()
    {
        _moveAction.action.Enable();
        _jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
        _jumpAction.action.Disable();
    }

    void Update()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0 )
        {
            _playerVelocity.y = -2f;
        }

        Vector2 input=_moveAction.action.ReadValue<Vector2>();
        Vector3 _move = new Vector3(input.x, 0, input.y);
        _move = Vector3.ClampMagnitude(_move, 1f);

        if (_move!=Vector3.zero)
        {
            transform.forward = _move;
        }

        if (_groundedPlayer && _jumpAction.action.WasPressedThisFrame()) 
        { 
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue); 
        }

        _playerVelocity.y += _gravityValue*Time.deltaTime;

        Vector3 _finalMove= _move*_playerSpeed+_playerVelocity.y*Vector3.up;
        _controller.Move(_finalMove*Time.deltaTime);
    }
}
