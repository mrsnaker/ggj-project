using System;
using DG.Tweening;
using UnityEngine;

namespace DNA
{
    public class DNA : MonoBehaviour
    {
        public Slot Slot { get; set; }
        private bool _isDrag = false;
        private void OnMouseDown()
        {
            switch (GameManager.NowStepLevel)
            {
                case GameState.FirstStage:
                    _isDrag = true;
                    break;
                case GameState.SecondStage:
                    break;
                case GameState.CalculateResult:
                case GameState.LoadingNextLevel:
                default:
                    return;
            }
        }

        private void OnMouseUp()
        {
            switch (GameManager.NowStepLevel)
            {
                case GameState.FirstStage:
                    _isDrag = false;
                    DNAManager.CheckNewDNAPos(this);
                    break;
                case GameState.SecondStage:
                    break;
                case GameState.CalculateResult:
                case GameState.LoadingNextLevel:
                default:
                    return;
            }
        }

        private void Update()
        {
            CheckDrag();
        }

        private void CheckDrag()
        {
            if(!_isDrag) return;
            
            var mousePos = Input.mousePosition;
            mousePos.z = transform.position.z;
            mousePos = GameManager.DNACamera.ScreenToWorldPoint(mousePos);
            mousePos.y = transform.position.y;
            if(transform.CheckBorderViewForDNA()) transform.position = mousePos;
        }

        public void ResetSmoothPos()
        {
            transform.DOMove(Slot.Pos, 0.2f).OnComplete(() => transform.SetParent(Slot.transform));
        }
    }
}
