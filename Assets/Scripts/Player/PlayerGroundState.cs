using UnityEngine;

namespace Player
{
    public class PlayerGroundState : PlayerState
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
            if (Input.GetKeyDown(KeyCode.Space) && Player.IsOnFloor())
                StateMachine.ChangeState(JumpPlayerState);
            if (Input.GetKeyDown(KeyCode.Mouse0))
                StateMachine.ChangeState(AttackPlayerState);
        }
    }
}
