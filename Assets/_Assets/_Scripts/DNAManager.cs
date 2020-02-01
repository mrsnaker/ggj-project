using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DNA
{
    public class DNAManager : MonoBehaviour
    {
        private static DNAManager _instance;
        private static DNAManager Instance => _instance ? _instance : _instance = FindObjectOfType<DNAManager>();
        private List<DNA> _dnaList = new List<DNA>();
        private List<Slot> _slotsList = new List<Slot>();
        public static List<DNA> DNAList 
        {
            get
            {
                if(Instance._dnaList.Count > 0) return Instance._dnaList;
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
            for(int i = 0; i < _dnaList.Count; i++)
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
        
        
}
}