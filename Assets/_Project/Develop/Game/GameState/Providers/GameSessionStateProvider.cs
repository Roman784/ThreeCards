using System.Collections.Generic;
using System;

namespace GameState
{
    public class GameSessionStateProvider
    {
        private List<GameSessionState> _states = new();

        public GameSessionStateProvider(GameSessionState state = null)
        {
            if (state != null)
                _states.Add(state);
        }

        public GameSessionState GetLastState()
        {
            if (_states.Count == 0)
                throw new NullReferenceException("The game session state list is empty.");

            return _states[_states.Count - 1];
        }
    }
}
