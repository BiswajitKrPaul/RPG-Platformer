namespace Player
{
    public class PlayerJumpState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            Player.SetVelocity(Velocity.x, PlayerConstants.JumpForce);
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
            if (Velocity.y > 0)
                StateMachine.ChangeState(AirPlayerState);
        }
    }
}
