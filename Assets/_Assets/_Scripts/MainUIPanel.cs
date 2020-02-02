using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DNA.UI
{
    public class MainUIPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _hintPanel;
        [SerializeField] private RectTransform _photoPanel;
        [SerializeField] private TextMeshProUGUI _stageText;
        [SerializeField] private Button _checkButton;
        [SerializeField] private TextMeshProUGUI _timerText;

        public RectTransform HintPanel => _hintPanel;
        public RectTransform PhotoPanel => _photoPanel;
        public TextMeshProUGUI StageText => _stageText;
        public Button CheckButton => _checkButton;
        public TextMeshProUGUI TimerText => _timerText;
        private float _startLocalYHint;

        public void HintButton()
        {
            if (_hintPanel.gameObject.activeSelf)
            {
                if (_startLocalYHint == 0f) _startLocalYHint = _hintPanel.localPosition.y;
                _hintPanel.DOLocalMoveY(_startLocalYHint - 300f, 0.5f)
                    .OnComplete(() => _hintPanel.gameObject.SetActive(false));
            }
            else
            {
                _hintPanel.gameObject.SetActive(true);
                _hintPanel.DOLocalMoveY(_startLocalYHint, 0.5f);
            }
        }

        public void Update()
        {
            if(Input.anyKey && _hintPanel.gameObject.activeSelf) HintButton();
        }
    }
}