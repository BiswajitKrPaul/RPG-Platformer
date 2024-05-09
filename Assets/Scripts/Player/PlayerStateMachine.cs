using UnityEngine;

namespace Player {
    public class PlayerStateMachine : MonoBehaviour {
        public PlayerState CurrentState { get; private set; }


        public void Initialize(PlayerState newState) {
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState newState) {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}