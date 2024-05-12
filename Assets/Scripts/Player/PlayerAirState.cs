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
            if (Player.IsOnFloor() && PlayerVelocity.y == 0)
                StateMachine.ChangeState(IdlePlayerState);
        }
    }
}
