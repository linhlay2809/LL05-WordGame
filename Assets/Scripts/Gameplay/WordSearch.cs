using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public enum GameState
    {
        PlayerTurn,
        AITurn
    }

    public enum LevelType
    {
        Easy,
        Hard
    }

    public class WordSearch : MonoBehaviour
    {
        private GameController _gameController;
        private UIManager _uiManager;
        private List<string> _wordList = new List<string>();

        [SerializeField] private Camera _camera;
        [SerializeField] private TMP_Text mainWordText;
        [SerializeField] private TMP_InputField inputField;
        
        private string _currentText;

        // Start is called before the first frame update
        void Start()
        {
            _gameController = GameController.Instance;
            _uiManager = UIManager.Instance;

            _gameController.OnAITurn += AITurn;
            _gameController.OnRestartGame += RestartGame;

            string fileWordsPath = Application.streamingAssetsPath + "/Words.txt";
            _wordList = File.ReadAllLines(fileWordsPath).ToList();
            _wordList = _wordList.ConvertAll(low => low.ToLower());
            _currentText = "";
            UpdateWordTextUI();
        }

        private void RestartGame()
        {
            _currentText = "";
            inputField.text = "";
            UpdateWordTextUI();
            _gameController.SetEndGame(false);
            _gameController.SetCurrentState(GameState.PlayerTurn);
            _gameController.SetFinalTurn(false);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ConfirmInputText();
            }
        }

        // Change color when entering true or false word
        private async void ExactWord(bool value)
        {
            if (value)
            {
                _camera.backgroundColor = Color.green;
                await Task.Delay(200); 
                _camera.backgroundColor = new Color32(135, 185, 190, 255);
            }
            else
            {
                _camera.backgroundColor = Color.red;
                await Task.Delay(200); 
                _camera.backgroundColor = new Color32(135, 185, 190, 255);
            }
            
        }
        private async void CheckWord()
        {
            // Find how many matching words in the Words.txt
            var words = _wordList.FindAll(
                s => s.Contains(_currentText) && s.StartsWith(_currentText));
            var wordCount = words.Count;

            // Debug words
            string debugText = null;
            foreach (var word in words)
            {
                debugText += $"{word} | ";
            }
            Debug.Log(wordCount +$": {debugText}");
            
            if (_gameController.FinalTurn) // the FinalTurn = true; when 1 player resign
            {
                if (wordCount >= 1) // Result => Current Turn WIN. if found more than 1 word
                {
                    _uiManager.ShowResult(_gameController.CurrentState);
                    _gameController.SetEndGame(true);
                    ExactWord(true);
                }
                else if (wordCount == 0) // Result => Draw. if no word is found
                {
                    _uiManager.ShowResult(null);
                    _gameController.SetEndGame(true);
                    ExactWord(false);
                }

                return;
            }

            if (wordCount > 1)
            {
                _gameController.SwitchTurn();
                ExactWord(true);
            }
            else if (wordCount == 1)
            {
                if (words[0].EndsWith(_currentText.Last().ToString())) // if the _currentText word is valid => Current Turn WIN
                {
                    _uiManager.ShowResult(_gameController.CurrentState, true);
                    _gameController.SetEndGame(true);
                    ExactWord(true);
                }
                else
                {
                    _gameController.SwitchTurn();
                    ExactWord(true);
                }
            }
            else
            {
                // If the wrong word is entered, delete the last character on _currentText and FinalTurn = true;
                await Task.Delay(1000);
                _currentText = _currentText.Remove(_currentText.Length - 1);
                UpdateWordTextUI();
                _gameController.SetFinalTurn(true);
                _gameController.SwitchTurn();
                ExactWord(false);
            }
        }

        // Call when CurrentState = AITurn
        private async void AITurn(LevelType levelType)
        {
            await Task.Delay(1000);
            if (levelType == LevelType.Easy)
            {
                char randomChar = (char)Random.Range('a', 'z');
                ChangeText(Char.ToString(randomChar));
            }
            else // LevelType = Hard
            {
                string wordToChangeText;
                bool boolean = Random.value > 0.5f;
                Debug.Log(boolean);
                if (boolean)
                {
                    var words = _wordList.FindAll(s => s.Contains(_currentText)
                                                       && s.StartsWith(_currentText.First().ToString())
                                                       && !s.EndsWith(_currentText.Last().ToString()));
                    var word = words[Random.Range(0, words.Count)];
                    var wordIndex = _currentText.Length;
                    wordToChangeText = Char.ToString(word[wordIndex]);
                }
                else
                {
                    char randomChar = (char)Random.Range('a', 'z');
                    wordToChangeText = Char.ToString(randomChar);
                }

                ChangeText(wordToChangeText);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void ConfirmInputText()
        {
            if (_gameController.EndGame) return;
            if (_gameController.CurrentState == GameState.AITurn) return;
            if (inputField.text.Length <= 0) return;
            ChangeText(inputField.text);
            inputField.text = "";
        }

        // Change currentText and CheckWord()
        private void ChangeText(string text)
        {
            _currentText += text.ToLower();
            UpdateWordTextUI();
            CheckWord();
        }

        // Update Main Word Text on UI 
        private void UpdateWordTextUI()
        {
            mainWordText.SetText(_currentText);
        }
    }
}