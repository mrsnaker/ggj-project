using System;
using DNA.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DNA
{
    public struct OnNextLevelLoad {}
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        private static GameManager Instance => _instance ? _instance : _instance = FindObjectOfType<GameManager>();

        [SerializeField] private Camera _dnaCamera;
        public static Camera DNACamera => Instance._dnaCamera;
        [SerializeField] private Camera _clientCamera;
        public static Camera ClientCamera => Instance._clientCamera;

        [SerializeField] private float _speedRotateDNA;
        public static float SpeedRotateDNA => Instance._speedRotateDNA;
        [SerializeField] private ResultPanel _resultPanel;
        public static ResultPanel ResultPanel => Instance._resultPanel;

        public static int NowLevelID = 0;
        public static GameState NowStepLevel { get; set; } = GameState.PlayStageA;
        
        private static float _timerGame = 0f;

        private void Awake()
        {
            LoadNextLevel();
        }
        
        private void OnEnable()
        {
            GlobalEvents<OnNextLevelLoad>.Happened += NextLevelLoaded;
        }
        
        private void OnDisable()
        {
            GlobalEvents<OnNextLevelLoad>.Happened -= NextLevelLoaded;
        }

        private void Update()
        {
            if (NowStepLevel == GameState.PlayStageA || NowStepLevel == GameState.PlayStageB)
            {
                _timerGame += Time.unscaledDeltaTime;
                
            }
        }

        private void NextLevelLoaded(OnNextLevelLoad obj)
        {
            _timerGame = 0f;
        }

        public static void LoadNextLevel()
        {
            NowLevelID++;
            SceneManager.LoadSceneAsync(NowLevelID);
            if(NowLevelID > 1) SceneManager.UnloadSceneAsync(NowLevelID - 1);
        }

        public void CheckResultButton()
        {
            DNAManager.CheckResult();
        }
    }
}