using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DNA.UI
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private Image _foregroundSlider;
        [SerializeField] private TextMeshProUGUI _resultText;

        public void Result(float result)
        {
            _foregroundSlider.fillAmount = 0;
            _foregroundSlider.DOFillAmount(result, 1f);
            var value = 0f;
            var target = Mathf.RoundToInt(result * 100);
            DOTween.To(() => value, x => value = x, target, 1f).OnUpdate(() => _resultText.SetText(value + "%"));
        }

        public void NextLevelButton()
        {
            GameManager.NowStepLevel = GameState.LoadingNextLevel;
            GameManager.LoadNextLevel();
        }
    }
}