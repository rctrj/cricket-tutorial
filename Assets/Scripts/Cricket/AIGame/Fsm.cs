using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cricket.AIGame
{
    public class Fsm : MonoBehaviour
    {
        [SerializeField] private GameState state = GameState.Idle;

        private readonly Dictionary<GameState, List<GameState>> _possibleTransitions = new()
        {
            { GameState.Idle, new List<GameState> { GameState.SettingAim, GameState.BallThrown } },
            { GameState.SettingAim, new List<GameState> { GameState.SettingPower } },
            { GameState.SettingPower, new List<GameState> { GameState.BallThrown } },
            {
                GameState.BallThrown, new List<GameState> { GameState.BallHit, GameState.Idle, GameState.InningsChange }
            },
            { GameState.BallHit, new List<GameState> { GameState.Idle, GameState.InningsChange } },
            { GameState.InningsChange, new List<GameState> { GameState.Idle } },
        };

        public GameState State
        {
            get => state;
            set
            {
                if (!_possibleTransitions[state].Contains(value))
                    throw new Exception("Transition not possible from: " + state + " to: " + value);

                var oldState = state;
                state = value;
                OnStateChange?.Invoke(oldState, value);
            }
        }

        public Action<GameState, GameState> OnStateChange;
    }
}