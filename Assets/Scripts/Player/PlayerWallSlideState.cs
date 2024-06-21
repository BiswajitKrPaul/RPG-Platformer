using UnityEngine;

namespace Player
{
    public class PlayerWallSlideState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void PhysicsProcess()
        {
            base.PhysicsProcess();
            Player.SetVelocity(Velocity.x, PlayerConstants.WallSlideSpeed);

            if (Player.IsOnFloor())
                StateMachine.ChangeState(IdlePlayerState);
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
