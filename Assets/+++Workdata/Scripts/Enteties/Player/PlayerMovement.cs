using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Grouping")]
    [DisplayColor(0, 0, 1), SerializeField] public float groupingRadius;

    [Header("Speed")] 
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float maxSpeedSubtraction;
    private float currentSpeed;
    
    [Header("Animation")]
    private Vector3 lastPosition;
    
    [Header("Input")]
    private Vector2 moveInput;
    public bool movementAllowed;
    
    private CachedZombieData cachedZombieData;

    private void Start()
    {
        cachedZombieData = GetComponent<CachedZombieData>();
    }
    
    private void Update()
    {
        if(movementAllowed)
            MoveZombie();
    }

    private void LateUpdate()
    {
        if(movementAllowed)
            MoveAnimationLateUpdate();
    }

    public void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>().normalized;
    }

    void MoveZombie()
    {
        //Subtract speed depending on how many zombies the player has as a horde
        float _speedSubtraction = cachedZombieData.ZombiePlayerHordeRegistry.Zombies.Count * .25f;
        currentSpeed = baseMoveSpeed - Mathf.Clamp(_speedSubtraction, .5f, maxSpeedSubtraction);

        float _moveDistance = currentSpeed * Time.deltaTime;
        
        Vector3 _moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        float _playerHeight = transform.localScale.z;
        float _playerRadius = transform.localScale.x;

        //The rest is just a check in which direction the player can move if collider are in front
        bool _canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * _playerHeight, _playerRadius, _moveDirection, _moveDistance);
        
        if (!_canMove)
        {
            Vector3 _moveDirectionX = new Vector3(_moveDirection.x, 0, 0);
            _canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * _playerHeight, _playerRadius, _moveDirectionX, _moveDistance);

            if (_canMove)
            {
                _moveDirection = _moveDirectionX;
            }
            else
            {
                Vector3 _moveDirectionZ = new Vector3(0, 0, _moveDirection.z);
                _canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * _playerHeight, _playerRadius, _moveDirectionZ, _moveDistance);

                if (_canMove)
                {
                    _moveDirection = _moveDirectionZ;
                }
            }
        }

        if (_canMove)
        {
            transform.position += _moveDirection * _moveDistance;
        }
    }

    void MoveAnimationLateUpdate()
    {
        var _currentSpeed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;

        lastPosition = transform.position;

        cachedZombieData.Animator.SetFloat("moveSpeed", _currentSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, groupingRadius);
    }
}
