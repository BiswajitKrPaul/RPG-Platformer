using Constants;
using UnityEngine;

namespace Player
{
    public class PlayerAttackState : PlayerState
    {
        private float _lastAttackTime;
        private int _comboCounter = 0;
        private const int ComboWindow = 2;

        public override void Enter()
        {
            base.Enter();
            if (_comboCounter > 2 || Time.time >= _lastAttackTime + ComboWindow)
                _comboCounter = 0;

            AnimatorPlayer.SetInteger(PlayerAnimationConstants.ComboCounter, _comboCounter);
        }

        public override void Exit()
        {
            base.Exit();
            Player.StartCoroutine(nameof(PlayerController.BusyFor), 0.15f);
            _lastAttackTime = Time.time;
            _comboCounter++;
            StateTimer = .1f;
        }

        public override void PhysicsProcess()
        {
            base.PhysicsProcess();
        }

        public override void Process()
        {
            base.Process();

            if (StateTimer < 0f)
                Player.SetZeroVelocity();

            if (TriggerCalled)
            {
                StateMachine.ChangeState(IdlePlayerState);
            }
        }
    }
}
