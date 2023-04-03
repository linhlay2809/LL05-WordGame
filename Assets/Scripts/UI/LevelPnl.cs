using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelPnl : MonoBehaviour
    {
        private GameController _gameController;
        [SerializeField] private Button easyBtn;
        [SerializeField] private Button hardBtn;

        private void Start()
        {
            _gameController = GameController.Instance;
            // AddListener to level button
            easyBtn.onClick.AddListener(() => OnSelectLevel(LevelType.Easy));
            hardBtn.onClick.AddListener(() => OnSelectLevel(LevelType.Hard));
        }

        // Call when select level button
        private void OnSelectLevel(LevelType levelType)
        {
            this.gameObject.SetActive(false);
            _gameController.SetLevelType(levelType);
            _gameController.OnRestartGame?.Invoke();
        }
    }
}