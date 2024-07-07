namespace Player
{
    public class PlayerIdleState : PlayerGroundState
    {
        public override void Enter()
        {
            base.Enter();
            Player.SetZeroVelocity();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void PhysicsProcess()
        {
            base.PhysicsProcess();
            if (XInput != 0 && Player.IsPlayerBusy == false)
                StateMachine.ChangeState(MovePlayerState);
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
