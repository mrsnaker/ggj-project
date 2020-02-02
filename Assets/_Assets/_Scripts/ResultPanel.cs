using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DNA.UI
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private Sprite[] _medalsSprites;
        [SerializeField] private Image _foregroundSlider;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private Image _medalsImage;
        [SerializeField] private GameObject _nextLeveButton;

        public void Result(float result)
        {
            _foregroundSlider.fillAmount = 0;
            _foregroundSlider.DOFillAmount(result, 1f);
            var value = 0f;
            var target = Mathf.RoundToInt(result * 100);
            DOTween.To(() => value, x => value = x, target, 1f).OnUpdate(() => _resultText.SetText(Math.Round(value, 1) + "%"));
            var numSprite = 0;
            if (result < 0.6f) numSprite = 0;
            else if (result <= 0.7f) numSprite = 1;
            else if (result <= 0.8f) numSprite = 2;
            else numSprite = 3;
            _medalsImage.sprite = _medalsSprites[numSprite];
            
            _nextLeveButton.SetActive(GameManager.NowLevelID < 3);
        }

        public void NextLevelButton()
        {
            GameManager.NowStepLevel = GameState.LoadingNextLevel;
            StartCoroutine(GameManager.LoadNextLevel());
        }
    }
}