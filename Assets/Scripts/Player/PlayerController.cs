using Constants;
using UnityEngine;

// ReSharper disable InvertIf

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("State Info")] [SerializeField]
        private PlayerStateMachine stateMachine;

        public PlayerIdleState idleState;
        public PlayerMoveState moveState;
        public PlayerJumpState jumpState;
        public PlayerAirState airState;
        public PlayerDashState dashState;

        [Header("Collision Info")] [SerializeField]
        private Transform groundRayCast;

        [SerializeField] private float rcDistance;
        [SerializeField] private float wRcDistance;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private Transform wallRayCast;

        [Header("Component Info")] public Animator animator;
        public Rigidbody2D playerRb;

        [Header("Move Info")] public float facingDirection = 1;
        public bool isFacingRight = true;
        public float dashDir;
        private float _dashTimerCooldown;

        public float XInput { get; private set; }


        private void Awake()
        {
            idleState.SetUp(stateMachine, this, AnimationConstants.Idle);
            moveState.SetUp(stateMachine, this, AnimationConstants.Move);
            jumpState.SetUp(stateMachine, this, AnimationConstants.Jump);
            airState.SetUp(stateMachine, this, AnimationConstants.Jump);
            dashState.SetUp(stateMachine, this, AnimationConstants.IsDashing);
        }

        private void Start()
        {
            stateMachine.Initialize(idleState);
        }

        private void Update()
        {
            stateMachine.CurrentState.Process();
            XInput = Input.GetAxisRaw("Horizontal");
            animator.SetFloat(AnimationConstants.YVelocity, playerRb.velocity.y);
            CheckDashInput();
            _dashTimerCooldown -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            stateMachine.CurrentState.PhysicsProcess();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(groundRayCast.position,
                new Vector2(groundRayCast.position.x, groundRayCast.position.y - rcDistance));
            Gizmos.DrawLine(wallRayCast.position,
                new Vector2(wallRayCast.position.x + wRcDistance * facingDirection, wallRayCast.position.y));
        }

        private void CheckDashInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _dashTimerCooldown < 0)
            {
                dashDir = Input.GetAxisRaw("Horizontal");
                if (dashDir == 0) dashDir = facingDirection;
                stateMachine.ChangeState(dashState);
                _dashTimerCooldown = 2f;
            }
        }

        /// <summary>
        /// Sets the player's velocity.
        /// </summary>
        /// <param name="x">Velocity on the x axis.</param>
        /// <param name="y">Velocity on the y axis.</param>
        public void SetVelocity(float x, float y)
        {
            playerRb.velocity = new Vector2(x, y);
            FlipController();
        }

        /// <summary>
        /// Sets the player's velocity to zero.
        /// </summary>
        public void SetZeroVelocity()
        {
            playerRb.velocity = new Vector2(0, 0);
        }

        /// <summary>
        /// Checks if the player is on the floor by casting a ray down from the groundRayCast position.
        /// </summary>
        /// <returns>True if the player is on the floor, false otherwise.</returns>
        public bool IsOnFloor()
        {
            return Physics2D.Raycast(groundRayCast.position, Vector2.down, rcDistance, whatIsGround);
        }

        /// <summary>
        /// Checks if a wall is detected in front of the player.
        /// </summary>
        /// <returns>True if a wall is detected, false otherwise.</returns>
        public bool IsWallDetected()
        {
            return Physics2D.Raycast(wallRayCast.position, Vector2.right * facingDirection,
                wRcDistance, whatIsGround);
        }


        /// <summary>
        /// Flips the player's orientation.
        /// </summary>
        /// <remarks>
        /// This method is responsible for flipping the player's orientation. It updates the facingDirection and isFacingRight properties,
        /// and then rotates the player's transform by 180 degrees around the y-axis.
        /// </remarks>
        private void Flip()
        {
            // Update the facingDirection to its opposite value
            facingDirection = -facingDirection;

            // Toggle the isFacingRight flag
            isFacingRight = !isFacingRight;

            // Rotate the player's transform by 180 degrees around the y-axis
            transform.Rotate(new Vector3(0, 180, 0));
        }

        // This function is responsible for controlling the flipping behavior of an object.
        // It checks the value of the 'XInput' variable and compares it to the current 'isFacingRight' state.
        // If 'XInput' is greater than 0 and 'isFacingRight' is false, or 'XInput' is less than 0 and 'isFacingRight' is true,
        // then the object is flipped by calling the 'Flip()' function.
        private void FlipController()
        {
            switch (XInput)
            {
                case > 0 when !isFacingRight:
                case < 0 when isFacingRight:
                    Flip();
                    break;
            }
        }
    }
}