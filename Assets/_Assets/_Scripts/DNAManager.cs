using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DNA
{
    public class DNAManager : MonoBehaviour
    {
        private static DNAManager _instance;
        private static DNAManager Instance => _instance ? _instance : _instance = FindObjectOfType<DNAManager>();

        [SerializeField] private List<CompareSlotDNA> _dnaCompare;
        [SerializeField] private List<DNA> _dnaList = new List<DNA>();
        [SerializeField] private List<Slot> _slotsList = new List<Slot>();

        public static List<DNA> DNAList
        {
            get
            {
                if (Instance._dnaList.Count > 0) return Instance._dnaList;
                Instance.UpdateDNAData();
                return Instance._dnaList;
            }
        }

        private void Awake()
        {
            UpdateDNAData();
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
        
        #endif
    }
}