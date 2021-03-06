﻿using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DNA
{
    public class DNAManager : MonoBehaviour
    {
        private static DNAManager _instance;
        private static DNAManager Instance => _instance ? _instance : _instance = FindObjectOfType<DNAManager>();

        [SerializeField] private float _timerOnLevel = 300f;
        public static float TimerOnLevel => Instance._timerOnLevel;
        [SerializeField] private RectTransform _photoPanel;
        public static RectTransform PhotoPanel => Instance._photoPanel;
        [SerializeField] private GameObject _dnaPrefab;
        [SerializeField] private List<CompareSlotDNA> _dnaCompare;
        public static List<CompareSlotDNA> DNACompare => Instance._dnaCompare;
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

        public static List<DNA> DNAList => Instance._dnaList;
        private float _lastRotate;

        private void OnEnable()
        {
            CalcCameraSize();
            RandomDNA();
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
            Instance.CheckStageA();
        }

        private void Update()
        {
            RotateAllDNAs();
        }

        private void RotateAllDNAs()
        {
            transform.Rotate(GameManager.SpeedRotateDNA * Time.deltaTime, 0f, 0f);
        }

        private void CheckStageA()
        {
            var count = 0;
            for (int i = 0; i < DNAList.Count; i++)
            {
                if (i == Instance._slotsList[i].DNA.ID)
                    count++;
            }

            if (count >= DNAList.Count)
            {
                GameManager.NowStepLevel = GameState.PlayStageB;
                Human.EnableStageBHuman();
            }
        }

        public static void CheckResult()
        {
            GameManager.NowStepLevel = GameState.CalculateResult;
            var allScore = 0f;
            var score = 0.5f / DNAList.Count;
            for (int i = 0; i < DNAList.Count; i++)
            {
                if (i == Instance._slotsList[i].DNA.ID)
                {
                    allScore += score;
                    if (DNACompare[i].IdBlend == -1) allScore += score;
                    else
                    {
                        var playerBlendResult = Human.StageBParameters[DNACompare[i].IdBlend].StageBMeshRenderer
                            .GetBlendShapeWeight(Human.StageBParameters[DNACompare[i].IdBlend].BlendShapeIndex);
                        var needResult = DNACompare[i].EndBlendValue;
                        allScore += score * (1f - (Mathf.Abs(playerBlendResult - needResult) * 0.01f));
                    }
                }
            }
            
            GameManager.ResultPanel.Result(allScore);
        }

        public static void RandomDNA()
        {
            var correctRandom = true;
            foreach (var dna in DNAList)
            {
                var random = Random.Range(0, DNAList.Count);
                dna.Slot.ChangeDNASlots(DNAList[random].Slot, true);
            }

            foreach (var dna in DNAList)
            {
                if (dna.Slot.ID == dna.ID) correctRandom = false;
            }

            if (!correctRandom) RandomDNA();
        }

        private void CalcCameraSize()
        {
            if (DNAList.Count <= 2) OrthographicSize = _startSizeCamera;
            else OrthographicSize = _startSizeCamera + (DNAList.Count - 2) * _stepSizeCameraForOneDNA;
        }
        
        #if UNITY_EDITOR


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
                _slotsList.Add(newSlotComponent);
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
                        _slotsList.Add(duplicate);
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
                    _slotsList.Add(duplicate);
                    i++;
                }
            }
            _slotsList.Sort(SortSlots);
            for (int i = 0; i < _slotsList.Count; i++)
            {
                _slotsList[i].transform.SetSiblingIndex(i);
                _slotsList[i].name = "Slots_" + i;
            }
            
            int SortSlots(Slot a, Slot b)
            {
                var dist = a.transform.position.x - b.transform.position.x;
                return Mathf.RoundToInt(dist);
            }
        }

        public void AddDNAs()
        {
            for (int i = 0; i < _slotsList.Count; i++)
            {
                var dna = Instantiate(_dnaPrefab, _slotsList[i].transform);
                dna.name = "DNA_" + i;
                dna.transform.localPosition = Vector3.zero;
                var dnaComp = dna.GetComponent<DNA>();
                dnaComp.ID = i;
                dnaComp.Slot = _slotsList[i];
                dnaComp.IDPartHuman = _dnaCompare[i].IdPartHuman;
                _slotsList[i].DNA = dnaComp;
                _dnaList.Add(dnaComp);
            }
        }

        public void RemoveAll()
        {
            var list = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
                list.Add(transform.GetChild(i).gameObject);
            list.ForEach(DestroyImmediate);
            _slotsList.Clear();
            _dnaList.Clear();
        }
        
        #endif
    }
}