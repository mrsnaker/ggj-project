using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DNA
{
    public class DNAManager : MonoBehaviour
    {
        private static DNAManager _instance;
        private static DNAManager Instance => _instance ? _instance : _instance = FindObjectOfType<DNAManager>();

        [SerializeField] private GameObject _dnaPrefab;
        [SerializeField] private List<CompareSlotDNA> _dnaCompare;
        [SerializeField] private List<DNA> _dnaList = new List<DNA>();
        [SerializeField] private List<Slot> _slotsList = new List<Slot>();
        [SerializeField] private float _orthographicSize;
        public float OrthographicSize
        {
            set
            {
                _orthographicSize = value;
                GameManager.DNACamera.orthographicSize = value;
            }
        }

        private const float _sizeDNA = 4.2f;
        private const float _startSizeCamera = 0.64f;
        private const float _stepSizeCameraForOneDNA = 0.3f;

        public static List<DNA> DNAList
        {
            get
            {
                if (Instance._dnaList.Count > 0) return Instance._dnaList;
                Instance.UpdateDNAData();
                return Instance._dnaList;
            }
        }

        private void OnEnable()
        {
            UpdateDNAData();
            CalcCameraSize();
        }

        private void UpdateDNAData()
        {
            Instance._dnaList.AddRange(Instance.GetComponentsInChildren<DNA>());
            if (_slotsList.Count > 0)
            {
                _slotsList.ForEach(x =>
                {
                    x.DNA.transform.SetParent(x.transform.parent);
                    DestroyImmediate(x.gameObject);
                });
                _slotsList.Clear();
            }
            for (int i = 0; i < _dnaList.Count; i++)
            {
                var newSlot = new GameObject("Slot" + i);
                var newSlotComponent = newSlot.AddComponent<Slot>();
                newSlot.transform.SetParent(_dnaList[i].transform.parent);
                newSlot.transform.localPosition = _dnaList[i].transform.localPosition;
                _dnaList[i].transform.SetParent(newSlot.transform);
                _slotsList.Add(newSlotComponent);
                _dnaList[i].Slot = newSlotComponent;
                newSlotComponent.DNA = _dnaList[i];
            }
        }

        public static void CheckNewDNAPos(DNA inDNA)
        {
            var slot = inDNA.Slot;
            var minDistance = inDNA.Slot.transform.DistanceXTo(inDNA.transform);
            foreach (var slotL in Instance._slotsList)
            {
                if (slotL.transform.DistanceXTo(inDNA.transform) < minDistance)
                {
                    minDistance = slotL.transform.DistanceXTo(inDNA.transform);
                    slot = slotL;
                }
            }

            inDNA.Slot.ChangeDNASlots(slot);
        }

        public static void CheckResult()
        {
            var allScore = 0f;
            var score = 1f / Instance._dnaList.Count;
            for (int i = 0; i < DNAList.Count; i++)
            {
                if (Instance._dnaCompare[i].IdDNA == Instance._slotsList[i].DNA.ID) allScore += score;
            }
            Debug.Log("Score: " + Mathf.RoundToInt(allScore));
        }

        public static void RandomDNA()
        {
            foreach (var dna in DNAList)
            {
                var random = Random.Range(0, DNAList.Count);
                dna.Slot.ChangeDNASlots(DNAList[random].Slot, true);
            }
        }

        private void CalcCameraSize()
        {
            if (DNAList.Count <= 2) OrthographicSize = _startSizeCamera;
            else OrthographicSize = _startSizeCamera + (DNAList.Count - 2) * _stepSizeCameraForOneDNA;
        }
        
        #if UNITY_EDITOR

        public void RebuildDNA()
        {
            if(_dnaCompare.Count <= 0) return;

            for (int i = 0; i < DNAList.Count; i++)
            {
                DNAList[i].ID = _dnaCompare[i].IdDNA;
                DNAList[i].Slot.ID = i;
            }

            RandomDNA();
        }

        public void InsertSlots()
        {
            var prevValue = 0f;
            var isEven = _dnaCompare.Count % 2 == 0;
            for (int i = 0, j = 0; i < _dnaCompare.Count; i++, j++)
            {
                var newSlot = new GameObject("Slot" + i);
                var newSlotComponent = newSlot.AddComponent<Slot>();
                newSlotComponent.ID = i;
                newSlot.transform.SetParent(transform);
                var pos = Vector3.zero;
                if (!isEven)
                {
                    if (i > 0)
                    {
                        pos.x = prevValue + _sizeDNA;
                        prevValue = pos.x;
                        newSlot.transform.localPosition = pos;
                        pos.x = -pos.x;
                        var duplicate = Instantiate(newSlot, newSlot.transform.parent).GetComponent<Slot>();
                        duplicate.transform.localPosition = pos;
                        duplicate.ID = i + 1;
                        i++;
                    } 
                    else newSlot.transform.localPosition = pos;
                }
                else
                {
                    if (i == 0) pos.x = _sizeDNA * 0.5f;
                    else pos.x = prevValue + _sizeDNA;
                    prevValue = pos.x;
                    newSlot.transform.localPosition = pos;
                    pos.x = -pos.x;
                    var duplicate = Instantiate(newSlot, newSlot.transform.parent).GetComponent<Slot>();
                    duplicate.transform.localPosition = pos;
                    duplicate.ID = i + 1;
                    i++;
                }
            }
            _slotsList.Sort();
        }

        public void RemoveAll()
        {
            for (int i = 0; i < transform.childCount; i++)
                DestroyImmediate(transform.GetChild(i));
            _slotsList.Clear();
            _dnaList.Clear();
        }
        
        #endif
    }
}