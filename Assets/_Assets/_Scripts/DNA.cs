using DG.Tweening;
using UnityEngine;

namespace DNA
{
    public class DNA : MonoBehaviour
    {
        [SerializeField] private int _id = 0;
        public int ID
        {
            get => _id;
            set => _id = value;
        }

        [SerializeField] private Slot _slot;
        public Slot Slot
        {
            get => _slot;
            set => _slot = value;
        }

        private bool _isDrag = false;
        private bool _isRotate = false;
        private float _lastPosX;
        private float _lastRotY;

        private SkinnedMeshRenderer _renderer;
        private SkinnedMeshRenderer Renderer => _renderer ? _renderer : _renderer = GetComponent<SkinnedMeshRenderer>();

        private void Awake()
        {
            Renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
        
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (GameManager.NowStepLevel)
                {
                    case GameState.PlayStage:
                        _isDrag = true;
                        _lastPosX = Input.mousePosition.x;
                        break;
                    case GameState.CalculateResult:
                    case GameState.LoadingNextLevel:
                    default:
                        return;
                }
            } else if (Input.GetMouseButtonDown(1))
            {
                switch (GameManager.NowStepLevel)
                {
                    case GameState.PlayStage:
                        _isRotate = true;
                        _lastRotY = Input.mousePosition.y;
                        break;
                    case GameState.CalculateResult:
                    case GameState.LoadingNextLevel:
                    default:
                        return;
                }
            }
        }

        private void Update()
        {
            CheckPress();
            CheckDrag();
            CheckRotate();
        }

        private void CheckPress()
        {
            if (Input.GetMouseButtonUp(0) && _isDrag)
            {
                switch (GameManager.NowStepLevel)
                {
                    case GameState.PlayStage:
                        _isDrag = false;
                        _lastPosX = 0;
                        DNAManager.CheckNewDNAPos(this);
                        break;
                    case GameState.CalculateResult:
                    case GameState.LoadingNextLevel:
                    default:
                        return;
                }
            }
            else if (Input.GetMouseButtonUp(1) && _isRotate)
            {
                switch (GameManager.NowStepLevel)
                {
                    case GameState.PlayStage:
                        _isRotate = false;
                        _lastRotY = 0f;
                        break;
                    case GameState.CalculateResult:
                    case GameState.LoadingNextLevel:
                    default:
                        return;
                }
            }
        }

        private void CheckDrag()
        {
            if(!_isDrag) return;
            
            var mousePos = Input.mousePosition;
            mousePos.z = transform.position.z;
            mousePos = GameManager.DNACamera.ScreenToWorldPoint(mousePos);
            mousePos.y = transform.position.y;
            if(transform.CheckBorderViewForDNA(Input.mousePosition.x > _lastPosX ? 1 : -1, mousePos)) transform.position = mousePos;
            _lastPosX = Input.mousePosition.x;
        }

        private void CheckRotate()
        {
            if(!_isRotate) return;
            
            var mousePos = Input.mousePosition;
            Renderer.SetBlendShapeWeight(0, Mathf.Clamp(Renderer.GetBlendShapeWeight(0) + (mousePos.y - _lastRotY) * GameManager.SpeedRotateDNA * Time.deltaTime, 0, 100));
            /*var rot = transform.localEulerAngles;
            rot.x += (mousePos.y - _lastRotY) * Time.deltaTime;
            transform.localEulerAngles = rot;*/
            _lastRotY = mousePos.y;
        }

        public void ResetSmoothPos(bool fast)
        {
            if (fast)
            {
                transform.position = Slot.Pos;
                transform.SetParent(Slot.transform);
            }
            else transform.DOMove(Slot.Pos, 0.2f).OnComplete(() => transform.SetParent(Slot.transform));
        }
    }
}
