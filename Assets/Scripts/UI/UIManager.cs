using System.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;

        public static UIManager Instance => _instance;

        [SerializeField] private LevelPnl levelPnl;
        [SerializeField] private ResultPnl resultPnl;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        // Active ResultPnl obj and show result content
        public async void ShowResult(GameState? state, bool isValidWord = false)
        {
            await Task.Delay(1000);

            resultPnl.gameObject.SetActive(true);

            resultPnl.ShowResult(state, isValidWord);
        }

        // Active LevelPnl Gameobject
        public void ShowLevelPnl()
        {
            levelPnl.gameObject.SetActive(true);
        }
        
    }
}