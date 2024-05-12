using UnityEngine;

namespace Player
{
    public class PlayerState : MonoBehaviour
    {
        private string _animationName;
        protected PlayerController Player;
        protected PlayerStateMachine StateMachine;

        protected float StateTimer;

        protected float XInput => Player.XInput;

        protected PlayerIdleState IdlePlayerState => Player.idleState;
        protected PlayerMoveState MovePlayerState => Player.moveState;
        protected PlayerJumpState JumpPlayerState => Player.jumpState;
        protected PlayerAirState AirPlayerState => Player.airState;
        protected PlayerDashState DashPlayerState => Player.dashState;

        private Animator PlayerAnimator => Player.animator;

        protected Vector2 PlayerVelocity => Player.playerRb.velocity;

        public void SetUp(
            PlayerStateMachine playerStateMachine,
            PlayerController playerController,
            string animName
        )
        {
            Player = playerController;
            StateMachine = playerStateMachine;
            _animationName = animName;
        }

        public virtual void Enter()
        {
            PlayerAnimator.SetBool(_animationName, true);
        }

        public virtual void Exit()
        {
            PlayerAnimator.SetBool(_animationName, false);
        }

        public virtual void PhysicsProcess() { }

        public virtual void Process()
        {
            StateTimer -= Time.deltaTime;
        }
    }
}
