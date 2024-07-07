using System.Collections;
using Constants;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region State Info

        [Header("State Info")]
        public PlayerStateMachine stateMachine;
        public PlayerIdleState idleState;
        public PlayerMoveState moveState;
        public PlayerJumpState jumpState;
        public PlayerAirState airState;
        public PlayerDashState dashState;
        public PlayerWallSlideState wallSlideState;
        public PlayerAttackState attackState;

        #endregion

        #region Collision Info

        [Header("Collision Info")]
        [SerializeField]
        private Transform groundRayCast;

        [SerializeField]
        private float rcDistance;

        [SerializeField]
        private float wRcDistance;

        [SerializeField]
        private LayerMask whatIsGround;

        [SerializeField]
        private Transform wallRayCast;

        #endregion

        #region Component Info

        [Header("Component Info")]
        public Animator animator;

        public Rigidbody2D playerRb;

        #endregion

        #region Move Info

        [Header("Move Info")]
        public float facingDirection = 1;

        public bool isFacingRight = true;
        public float dashDir;
        private float _dashTimerCooldown;
        public bool IsPlayerBusy { get; private set; }

        #endregion


        public float XInput { get; private set; }

        private void Awake()
        {
            idleState.SetUp(stateMachine, this, PlayerAnimationConstants.Idle);
            moveState.SetUp(stateMachine, this, PlayerAnimationConstants.Move);
            jumpState.SetUp(stateMachine, this, PlayerAnimationConstants.Jump);
            airState.SetUp(stateMachine, this, PlayerAnimationConstants.Jump);
            dashState.SetUp(stateMachine, this, PlayerAnimationConstants.IsDashing);
            wallSlideState.SetUp(stateMachine, this, PlayerAnimationConstants.IsWallSliding);
            attackState.SetUp(stateMachine, this, PlayerAnimationConstants.IsAttacking);
        }

        private void Start()
        {
            stateMachine.Initialize(idleState);
        }

        private void Update()
        {
            stateMachine.CurrentState.Process();
            XInput = Input.GetAxisRaw("Horizontal");
            animator.SetFloat(PlayerAnimationConstants.YVelocity, playerRb.velocity.y);
            CheckDashInput();
            _dashTimerCooldown -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            stateMachine.CurrentState.PhysicsProcess();
        }

        public IEnumerator BusyFor(float timeInSeconds)
        {
            IsPlayerBusy = true;
            yield return new WaitForSeconds(timeInSeconds);
            IsPlayerBusy = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(
                groundRayCast.position,
                new Vector2(groundRayCast.position.x, groundRayCast.position.y - rcDistance)
            );
            Gizmos.DrawLine(
                wallRayCast.position,
                new Vector2(
                    wallRayCast.position.x + wRcDistance * facingDirection,
                    wallRayCast.position.y
                )
            );
        }

        /// <summary>
        /// Checks for dash input and changes the player's state to the dash state if the input is valid.
        /// </summary>
        /// <remarks>
        /// This method is called to check if the left shift key is pressed and if the dash cooldown is less than 0.
        /// If both conditions are true, it sets the dash direction based on the horizontal input or the facing direction if the input is 0.
        /// It then changes the player's state to the dash state and sets the dash cooldown to 2.
        /// </remarks>
        private void CheckDashInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _dashTimerCooldown < 0 && !IsWallDetected())
            {
                dashDir = Input.GetAxisRaw("Horizontal");
                if (dashDir == 0f)
                    dashDir = facingDirection;
                stateMachine.ChangeState(dashState);
                _dashTimerCooldown = 2f;
            }
        }

        /// <summary>
        /// Sets the player's velocity.
        /// </summary>
        /// <param name="x">Velocity on the x-axis.</param>
        /// <param name="y">Velocity on the y-axis.</param>
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
            playerRb.velocity = new Vector2(0f, 0f);
        }

        /// <summary>
        /// Checks if the player is on the floor by casting a ray down from the groundRayCast position.
        /// </summary>
        /// <returns>True if the player is on the floor, false otherwise.</returns>
        public bool IsOnFloor()
        {
            return Physics2D.Raycast(
                groundRayCast.position,
                Vector2.down,
                rcDistance,
                whatIsGround
            );
        }

        /// <summary>
        /// Checks if a wall is detected in front of the player.
        /// </summary>
        /// <returns>True if a wall is detected, false otherwise.</returns>
        public bool IsWallDetected()
        {
            return Physics2D.Raycast(
                wallRayCast.position,
                Vector2.right * facingDirection,
                wRcDistance,
                whatIsGround
            );
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
            transform.Rotate(new Vector3(0f, 180f, 0f));
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

        /// <summary>
        /// Calls the 'OnAnimationTriggerCalled' method of the current state in the state machine.
        /// </summary>
        private void OnAnimationTriggerCalled()
        {
            stateMachine.CurrentState.OnAnimationTriggerCalled();
        }
    }
}
