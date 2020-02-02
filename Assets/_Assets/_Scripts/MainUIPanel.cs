using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DNA.UI
{
    public class MainUIPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _hintPanel;
        [SerializeField] private TextMeshProUGUI _stageText;
        [SerializeField] private Button _checkButton;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private RectTransform _startLogoObject;

        public RectTransform HintPanel => _hintPanel;
        public RectTransform PhotoPanel => DNAManager.PhotoPanel;
        public TextMeshProUGUI StageText => _stageText;
        public Button CheckButton => _checkButton;
        public TextMeshProUGUI TimerText => _timerText;
        private float _startLocalYHint;
        private float _startLocalXPhoto;

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
            if (!Input.anyKey || !_hintPanel.gameObject.activeSelf) return;
            HintButton();
            if (GameManager.NowLevelID == 1 && GameManager.NowStepLevel == GameState.LoadingNextLevel)
                GameManager.NowStepLevel = GameState.PlayStageA;
            _startLogoObject.DOLocalMoveY(_startLogoObject.localPosition.y + 1000f, 1f).SetEase(Ease.InElastic)
                .OnComplete(() => _startLogoObject.gameObject.SetActive(false));
        }

        public void PhotoPanelState(bool state, bool fast = false)
        {
            if(PhotoPanel == null) return;
            
            if (!state)
            {
                if (_startLocalXPhoto == 0f) _startLocalXPhoto = PhotoPanel.localPosition.x;
                if(!fast) PhotoPanel.DOLocalMoveX(_startLocalXPhoto + 1500f, 2f)
                    .OnComplete(() => PhotoPanel.gameObject.SetActive(false));
                else PhotoPanel.localPosition = new Vector3(_startLocalXPhoto + 1500f, PhotoPanel.localPosition.y, PhotoPanel.localPosition.z);
            }
            else
            {
                PhotoPanel.gameObject.SetActive(true);
                if (!fast) PhotoPanel.DOLocalMoveX(_startLocalXPhoto, 1f);
                else
                    PhotoPanel.localPosition = new Vector3(_startLocalXPhoto, PhotoPanel.localPosition.y,
                        PhotoPanel.localPosition.z);
            }
        }
    }
}