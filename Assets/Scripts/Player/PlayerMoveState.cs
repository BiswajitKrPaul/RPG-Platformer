namespace Player
{
    public class PlayerMoveState : PlayerGroundState
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
            Player.SetVelocity(PlayerConstants.MoveSpeed * XInput, PlayerVelocity.y);
            if (XInput == 0 || Player.IsWallDetected())
                StateMachine.ChangeState(IdlePlayerState);
        }
    }
}
