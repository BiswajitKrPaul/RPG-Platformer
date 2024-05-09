using Constants;
using UnityEngine;

// ReSharper disable InvertIf

namespace Player {
    public class PlayerController : MonoBehaviour {
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


        private void Awake() {
            idleState.SetUp(stateMachine, this, AnimationConstants.Idle);
            moveState.SetUp(stateMachine, this, AnimationConstants.Move);
            jumpState.SetUp(stateMachine, this, AnimationConstants.Jump);
            airState.SetUp(stateMachine, this, AnimationConstants.Jump);
            dashState.SetUp(stateMachine, this, AnimationConstants.IsDashing);
        }

        private void Start() {
            stateMachine.Initialize(idleState);
        }

        private void Update() {
            stateMachine.CurrentState.Process();
            XInput = Input.GetAxisRaw("Horizontal");
            animator.SetFloat(AnimationConstants.YVelocity, playerRb.velocity.y);
            CheckDashInput();
            _dashTimerCooldown -= Time.deltaTime;
        }

        private void FixedUpdate() {
            stateMachine.CurrentState.PhysicsProcess();
        }

        private void OnDrawGizmos() {
            Gizmos.DrawLine(groundRayCast.position,
                new Vector2(groundRayCast.position.x, groundRayCast.position.y - rcDistance));
            Gizmos.DrawLine(wallRayCast.position,
                new Vector2(wallRayCast.position.x + wRcDistance * facingDirection, wallRayCast.position.y));
        }

        private void CheckDashInput() {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _dashTimerCooldown < 0) {
                dashDir = Input.GetAxisRaw("Horizontal");
                if (dashDir == 0) dashDir = facingDirection;
                stateMachine.ChangeState(dashState);
                _dashTimerCooldown = 2f;
            }
        }

        public void SetVelocity(float x, float y) {
            playerRb.velocity = new Vector2(x, y);
            FlipController();
        }

        public void SetZeroVelocity() {
            playerRb.velocity = new Vector2(0, 0);
        }

        public bool IsOnFloor() {
            return Physics2D.Raycast(groundRayCast.position, Vector2.down, rcDistance, whatIsGround);
        }

        public bool IsWallDetected() {
            return Physics2D.Raycast(wallRayCast.position, Vector2.right * facingDirection,
                wRcDistance, whatIsGround);
        }


        private void Flip() {
            facingDirection = facingDirection * -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }

        private void FlipController() {
            switch (XInput) {
                case > 0 when !isFacingRight:
                case < 0 when isFacingRight:
                    Flip();
                    break;
            }
        }
    }
}