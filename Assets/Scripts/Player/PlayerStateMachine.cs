using UnityEngine;

namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public PlayerState CurrentState { get; private set; }

        /// <summary>
        /// Initializes the player state machine with the provided state.
        /// </summary>
        /// <param name="newState">The initial state of the player.</param>
        public void Initialize(PlayerState newState)
        {
            CurrentState = newState;
            CurrentState.Enter();
        }

        // Changes the current state of the player to the new state provided.
        public void ChangeState(PlayerState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
