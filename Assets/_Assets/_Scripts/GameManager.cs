﻿using System;
using DNA.UI;
using UnityEngine;

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

        private static int _nowLevelID = 0;
        public static GameState NowStepLevel { get; private set; } = 0;

        private void OnEnable()
        {
            GlobalEvents<OnNextLevelLoad>.Happened += NextLevelLoaded;
        }
        
        private void OnDisable()
        {
            GlobalEvents<OnNextLevelLoad>.Happened -= NextLevelLoaded;
        }

        private void NextLevelLoaded(OnNextLevelLoad obj)
        {
            
        }

        public void CheckResultButton()
        {
            DNAManager.CheckResult();
        }
    }
}