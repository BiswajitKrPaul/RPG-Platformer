namespace Player {
    public class PlayerDashState : PlayerState {
        public override void Enter() {
            base.Enter();
            StateTimer = PlayerConstants.DashTimer;
        }

        public override void Exit() {
            base.Exit();
            Player.SetZeroVelocity();
        }

        public override void PhysicsProcess() {
            base.PhysicsProcess();
        }

        public override void Process() {
            base.Process();
            Player.SetVelocity(PlayerConstants.DashSpeed * Player.dashDir, 0);
            if (StateTimer < 0) {
                if (!Player.IsOnFloor()) {
                    StateMachine.ChangeState(AirPlayerState);
                    return;
                }

                StateMachine.ChangeState(IdlePlayerState);
            }
        }
    }
}