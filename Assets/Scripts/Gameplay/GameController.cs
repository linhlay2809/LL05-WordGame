using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class GameController : MonoBehaviour
    {
        private static GameController _instance;
        public static GameController Instance => _instance;

        [field: SerializeField] public GameState CurrentState { get; private set; }
        [field: SerializeField] public LevelType LevelType { get; private set; }
        [field: SerializeField] public bool FinalTurn { get; private set; }
        [field: SerializeField] public bool EndGame { get; private set; }

        public Action<LevelType> OnAITurn;
        public Action OnRestartGame;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public void SetCurrentState(GameState state)
        {
            this.CurrentState = state;
        }

        public void SetEndGame(bool endGame)
        {
            this.EndGame = endGame;
        }

        public void SetLevelType(LevelType type)
        {
            this.LevelType = type;
        }

        public void SetFinalTurn(bool value)
        {
            this.FinalTurn = value;
        }

        public void SwitchTurn()
        {
            CurrentState = CurrentState == GameState.PlayerTurn ? GameState.AITurn : GameState.PlayerTurn;

            if (CurrentState == GameState.AITurn)
            {
                OnAITurn?.Invoke(LevelType);
            }
        }
    }
}