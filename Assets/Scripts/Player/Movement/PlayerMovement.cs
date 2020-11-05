using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Global Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private SphereCollider groundChecker;
    [SerializeField] private SphereCollider collisionDetections;

    [Header("Touch Movement")]
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private GameObject indicatorGO;

    [Header("Dash Movement")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashTurnSmoothTime = 0.1f;
    private Vector3 _dashPosition;
    private Vector3 _dashDirection;
    private float _dashingTime;

    private Vector3 _nextPosition;
    private GameObject _currentIndicator;
    private bool _useTouch = true;
        
    private float _turnSmoothVelocity;
    private const float Gravity = -9.81f;
    private Vector3 _velocity;


    private void Update()
    {
        if (_dashingTime <= 0) {
            // Keyboard Movement 
            float horizontal = Input.GetAxisRaw("Horizontal"); // Input Q & D
            float vertical = Input.GetAxisRaw("Vertical"); // Input Z & S

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            _useTouch = !(direction.magnitude >= 0.1f);
            bool dash = false;

            // Touch Movement
            if (Input.GetKeyDown(KeyCode.F)) {
                _nextPosition = GetPosition();
                SpawnIndicator();
                _useTouch = true;
            } else if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0)) {
                _dashPosition = GetPosition();
                dash = true;
                _useTouch = false;
            }

            if (Vector3.Distance(transform.position, _nextPosition) <= 0.1f) {
                StopPlayer();
            }

            // Movement execution
            if (_useTouch) {
                if (_nextPosition != Vector3.zero && transform.position != _nextPosition) {
                    Move((_nextPosition - transform.position).normalized);
                }
            } else if (dash) {
                if (_dashPosition != Vector3.zero && transform.position != _dashPosition) {
                    Dash((_dashPosition - transform.position).normalized);
                }
            } else {
                if (_currentIndicator != null) {
                    Destroy(_currentIndicator);
                }

                _nextPosition = Vector3.zero;
                Move(direction);
            }

            // Apply gravity
            Collider[] colliders = Physics.OverlapSphere(groundChecker.transform.position, groundChecker.radius);
            bool isGrounded = colliders.Any(col => col.gameObject.CompareTag("Ground"));
            if (!isGrounded) {
                _velocity.y += Gravity * Time.deltaTime;
                controller.Move(_velocity * Time.deltaTime);
            } else {
                _velocity = Vector3.zero;
            }
        } else {
            RotatePlayer(_dashDirection, dashTurnSmoothTime);
            controller.Move(_dashDirection * (dashSpeed * Time.deltaTime));
            _dashingTime -= Time.deltaTime;
            _nextPosition = transform.position;
        }
    }

    /// <summary>
    ///     This function move player in direction given in parameter
    /// </summary>
    /// <param name="direction">The direction of the player</param>
    private void Move(Vector3 direction)
    {
        RotatePlayer(direction, turnSmoothTime);
        controller.Move(direction * (speed * Time.deltaTime));
    }

    /// <summary>
    ///     This function move fastly player in direction given in parameter
    /// </summary>
    /// <param name="direction">The direction of the dash</param>
    private void Dash(Vector3 direction)
    {
        _dashDirection = direction;
        controller.Move(direction * (dashSpeed * Time.deltaTime));
        _dashingTime = dashDuration;
    }
    
    /// <summary>
    ///     This function rotate player to direction given in parameter
    /// </summary>
    /// <param name="direction">The direction of the player</param>
    /// <param name="turnSmooth">Smoothness of turn</param>
    private void RotatePlayer(Vector3 direction, float turnSmooth)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmooth);
            
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
    
    /// <summary>
    ///     This function is use to get position of mouse click
    /// </summary>
    /// <returns>Position of the mouse click in Vector3</returns>
    private Vector3 GetPosition()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ~ignoreMask)) {
            if (hit.transform.CompareTag("Ground")) {
                return hit.point;
            }
        }

        return transform.position;
    }
    
    /// <summary>
    ///     This function instantiate an indicator at the position of the click
    /// </summary>
    private void SpawnIndicator()
    {
        if (_currentIndicator != null) {
            Destroy(_currentIndicator);
        }
        _currentIndicator = Instantiate(indicatorGO, new Vector3(_nextPosition.x, transform.position.y, _nextPosition.z), Quaternion.identity);
        Destroy(_currentIndicator, 1f);
    }
    
    /// <summary>
    ///     This function stop movement of the player
    /// </summary>
    public void StopPlayer()
    {
        controller.enabled = false;
        controller.enabled = true;
        if (_currentIndicator != null) {
            Destroy(_currentIndicator);
        }
    }
}