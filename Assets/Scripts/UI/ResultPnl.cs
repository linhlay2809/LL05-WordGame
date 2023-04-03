using System;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ResultPnl : MonoBehaviour
    {
        private GameController _gameController;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text contentText;

        private void Start()
        {
            _gameController = GameController.Instance;
        }

        public void ShowResult(GameState? state, bool isValidWord = false)
        {
            string content = "";
            switch (state)
            {
                case GameState.PlayerTurn:
                    content = "Player Win";
                    break;
                case GameState.AITurn:
                    content = "AI Win";
                    break;
                default:
                    content = "Draw";
                    break;
            }

            if (isValidWord)
            {
                contentText.gameObject.SetActive(true);
                contentText.SetText("No more letter that can be appended to develop into a valid word,");
            }
            resultText.SetText(content);
        }
        public void OnSelectResultBtn()
        {
            this.gameObject.SetActive(false);
            UIManager.Instance.ShowLevelPnl();
        }
    }
}