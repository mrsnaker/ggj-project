using System;
using DG.Tweening;
using UnityEngine;

namespace DNA
{
    public class DNA : MonoBehaviour
    {
        public Slot Slot { get; set; }
        private bool _isDrag = false;
        private bool _isRotate = false;
        private float _lastPosX;
        private float _lastRotY;
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
            var rot = transform.localEulerAngles;
            rot.x += (mousePos.y - _lastRotY) * Time.deltaTime;
            transform.localEulerAngles = rot;
            _lastRotY = mousePos.y;
        }

        public void ResetSmoothPos()
        {
            transform.DOMove(Slot.Pos, 0.2f).OnComplete(() => transform.SetParent(Slot.transform));
        }
    }
}
