namespace Player
{
    public class PlayerAirState : PlayerState
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
        }

        public override void Process()
        {
            base.Process();
            if (Player.IsOnFloor() && Velocity.y == 0)
                StateMachine.ChangeState(IdlePlayerState);

            if (!Player.IsOnFloor() && Player.IsWallDetected())
            {
                StateMachine.ChangeState(WallSlidePlayerState);
            }
        }
    }
}
