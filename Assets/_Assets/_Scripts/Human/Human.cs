﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Human : MonoBehaviour
{
    private static Human _instance;
    private static Human Instance => _instance ? _instance : FindObjectOfType<Human>();

    [SerializeField] private HumanPartSlot _humanPartSlotPrefab;
    [SerializeField] private List<HumanPart> _humanParts;

    [SerializeField] private Dictionary<HumanPartSize, List<HumanPartSlot>> _humanSlots = new Dictionary<HumanPartSize, List<HumanPartSlot>>();

    private static System.Random rng = new System.Random();

    private void Awake()
    {
        _humanParts = new List<HumanPart>();
        _humanParts = GetComponentsInChildren<HumanPart>().ToList();

        for (int i = 0; i < _humanParts.Count; i++)
        {
            var humanPart = _humanParts[i];
            var humanSlot = Instantiate(_humanPartSlotPrefab);
            humanSlot.name = humanPart.name + "Slot";
            humanSlot.ID = humanPart.ID;
            humanSlot.Size = humanPart.Size;
            humanSlot.Direction.localPosition = humanPart.Direction.localPosition;
            humanSlot.Direction.localRotation = humanPart.Direction.localRotation;

            humanSlot.transform.parent = humanPart.transform.parent;
            humanSlot.transform.localPosition = humanPart.transform.localPosition;
            humanSlot.transform.localRotation = humanPart.transform.localRotation;
            //scale

            humanPart.transform.parent = humanSlot.transform;
            humanPart.transform.localPosition = Vector3.zero;
            humanPart.transform.localRotation = Quaternion.identity;

            humanSlot.CurrentPart = humanPart;
            if (!_humanSlots.ContainsKey(humanSlot.Size)) _humanSlots.Add(humanSlot.Size, new List<HumanPartSlot>());
            _humanSlots[humanSlot.Size].Add(humanSlot);
        }

        var randomSlot1 = Random.Range(0, _humanSlots[HumanPartSize.Big].Count);
        var randomSlot2 = Random.Range(0, _humanSlots[HumanPartSize.Big].Count);
        SwapParts(_humanSlots[HumanPartSize.Big][randomSlot1], _humanSlots[HumanPartSize.Big][randomSlot2]);
        //ShuffleParts(_humanPartSlots);
    }

    private static HumanPartSlot GetRightSlotFromPart(HumanPart part)
    {
        return Instance._humanSlots[part.Size].Find(p => p.ID == part.ID);
    }

    private static HumanPartSlot GetCurrentSlotFromPart(HumanPart part)
    {
        return Instance._humanSlots[part.Size].Find(ps => ps.CurrentPart == part);
    }

    private static HumanPart GetRightPartFromSlot(HumanPartSlot partSlot)
    {
        return Instance._humanParts.Find(p => p.ID == partSlot.ID);
    }

    private static HumanPart GetCurrentPartFromSlot(HumanPartSlot partSlot)
    {
        return Instance._humanParts.Find(p => p == partSlot.CurrentPart);
    }

    private static void SwapParts(HumanPartSlot a, HumanPartSlot b)
    {
        Debug.Log(a.name + "--" + b.name);
        if (a.Size != b.Size) return;
        var temp = a.CurrentPart;
        a.CurrentPart = b.CurrentPart;
        a.CurrentPart.transform.parent = a.transform;
        a.CurrentPart.transform.localPosition = Vector3.zero;
        a.CurrentPart.transform.localRotation = Quaternion.identity;

        var diff = a.Direction.localRotation.eulerAngles - a.CurrentPart.Direction.localRotation.eulerAngles;
        a.CurrentPart.transform.localRotation = Quaternion.Euler(diff);

        b.CurrentPart = temp;
        b.CurrentPart.transform.parent = b.transform;
        b.CurrentPart.transform.localPosition = Vector3.zero;
        b.CurrentPart.transform.localRotation = Quaternion.identity;

        diff = b.Direction.localRotation.eulerAngles - b.CurrentPart.Direction.localRotation.eulerAngles;
        b.CurrentPart.transform.localRotation = Quaternion.Euler(diff);
    }

    private static void ShuffleParts(List<HumanPartSlot> slots)
    {
        var n = slots.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            SwapParts(slots[k], slots[n]);
        }
    }
}
