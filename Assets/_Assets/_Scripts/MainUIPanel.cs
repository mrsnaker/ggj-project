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
    }
}