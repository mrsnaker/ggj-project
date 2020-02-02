using System;
using System.Collections;
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

        [SerializeField] private AudioSource _voicesAudioSource;
        public static AudioSource VoicesAudioSource => Instance._voicesAudioSource;

        [SerializeField] private float _speedRotateDNA;
        public static float SpeedRotateDNA => Instance._speedRotateDNA;
        [SerializeField] private ResultPanel _resultPanel;
        public static ResultPanel ResultPanel => Instance._resultPanel;
        [SerializeField] private MainUIPanel _mainPanel;
        public static MainUIPanel MainPanel => Instance._mainPanel;

        public static int NowLevelID = 0;
        private static GameState _nowStepLevel = GameState.PlayStageA;

        public static GameState NowStepLevel
        {
            get => _nowStepLevel;
            set
            {
                _nowStepLevel = value;
                Instance.CheckState();
            }
        }

        private static float _timerGame = 0f;

        private void Awake()
        {
            StartCoroutine(LoadNextLevel());
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
                var timeSpan = new TimeSpan(0,0,0,Mathf.RoundToInt(DNAManager.TimerOnLevel - _timerGame));
                MainPanel.TimerText.SetText($"Time: {timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
            }
            else MainPanel.TimerText.SetText("Time: 00:00");
        }

        private void CheckState()
        {
            switch (NowStepLevel)
            {
                case GameState.PlayStageA:
                    MainPanel.CheckButton.gameObject.SetActive(false);
                    MainPanel.StageText.SetText("Stage A");
                    MainPanel.PhotoPanelState(false, true);
                    break;
                case GameState.PlayStageB:
                    MainPanel.CheckButton.gameObject.SetActive(true);
                    MainPanel.StageText.SetText("Stage B");
                    MainPanel.PhotoPanelState(true);
                    break;
                case GameState.CalculateResult:
                    MainPanel.CheckButton.gameObject.SetActive(false);
                    ResultPanel.gameObject.SetActive(true);
                    MainPanel.StageText.SetText("Result");
                    MainPanel.PhotoPanelState(false);
                    break;
                case GameState.LoadingNextLevel:
                    MainPanel.PhotoPanelState(false);
                    ResultPanel.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void NextLevelLoaded(OnNextLevelLoad obj)
        {
            _timerGame = 0f;
        }

        public static IEnumerator LoadNextLevel()
        {
            NowLevelID++;
            var async = SceneManager.LoadSceneAsync(NowLevelID, LoadSceneMode.Additive);
            yield return new WaitUntil(() => async.isDone);
            NowStepLevel = NowLevelID == 1 ? GameState.LoadingNextLevel : GameState.PlayStageA;
            if(NowLevelID > 1) SceneManager.UnloadSceneAsync(NowLevelID - 1);
        }

        public void CheckResultButton()
        {
            DNAManager.CheckResult();
        }
    }
}