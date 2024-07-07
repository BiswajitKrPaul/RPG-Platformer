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
        protected PlayerWallSlideState WallSlidePlayerState => Player.wallSlideState;
        protected PlayerAttackState AttackPlayerState => Player.attackState;

        protected Animator AnimatorPlayer => Player.animator;

        protected Vector2 Velocity => Player.playerRb.velocity;

        protected bool TriggerCalled;

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
            AnimatorPlayer.SetBool(_animationName, true);
            TriggerCalled = false;
        }

        public virtual void Exit()
        {
            AnimatorPlayer.SetBool(_animationName, false);
        }

        public virtual void PhysicsProcess() { }

        public virtual void Process()
        {
            StateTimer -= Time.deltaTime;
        }

        public void OnAnimationTriggerCalled()
        {
            TriggerCalled = true;
        }
    }
}
