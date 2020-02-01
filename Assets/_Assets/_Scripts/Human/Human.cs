using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Human : MonoBehaviour
{
    private static Human _instance;
    public static Human Instance => _instance ? _instance : FindObjectOfType<Human>();

    [SerializeField] private HumanPartSlot _humanPartSlotPrefab;
    [SerializeField] private List<HumanPart> _humanParts;
    [SerializeField] private Dictionary<HumanPartSize, List<HumanPartSlot>> _humanSlots = new Dictionary<HumanPartSize, List<HumanPartSlot>>();

    [SerializeField] private SkinnedMeshRenderer _chest;

    private static System.Random rng = new System.Random();
    private Sequence _chestAnimation;

    private void Awake()
    {
        _chestAnimation = StartChestAnimation();
        _humanParts = new List<HumanPart>();
        _humanParts = GetComponentsInChildren<HumanPart>().ToList();

        for (int i = 0; i < _humanParts.Count; i++)
        {
            var humanPart = _humanParts[i];
            humanPart.ID = i;
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
        var randomSlot2 = (randomSlot1 + Random.Range(1, _humanSlots[HumanPartSize.Big].Count)) % _humanSlots[HumanPartSize.Big].Count;
        //Debug.Log(randomSlot1 + " " + randomSlot2);
        //SwapParts(_humanSlots[HumanPartSize.Big][randomSlot1], _humanSlots[HumanPartSize.Big][randomSlot2]);
        SwapParts(_humanSlots[HumanPartSize.Big][1], _humanSlots[HumanPartSize.Big][2]);
        //ShuffleParts(_humanPartSlots);
    }

    private Sequence StartChestAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(()=> { _chest.SetBlendShapeWeight(0, 0); })
            .Append(DOTween.To(() => _chest.GetBlendShapeWeight(0), x => _chest.SetBlendShapeWeight(0, x), 100f, 1.5f))
            .Append(DOTween.To(() => _chest.GetBlendShapeWeight(0), x => _chest.SetBlendShapeWeight(0, x), 0f, 0.75f))
            .SetLoops(-1);
    }

    private void StopChestAnimation()
    {

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
        //diff = a.CurrentPart.Direction.localRotation.eulerAngles - a.Direction.localRotation.eulerAngles;
        //diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), Mathf.Abs(diff.z));
        a.CurrentPart.transform.localRotation = a.Direction.localRotation;//Quaternion.Euler(diff);

        b.CurrentPart = temp;
        b.CurrentPart.transform.parent = b.transform;
        b.CurrentPart.transform.localPosition = Vector3.zero;
        b.CurrentPart.transform.localRotation = Quaternion.identity;

        diff = b.Direction.localRotation.eulerAngles - b.CurrentPart.Direction.localRotation.eulerAngles;
        //diff = b.CurrentPart.Direction.localRotation.eulerAngles - b.Direction.localRotation.eulerAngles;
        //diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), Mathf.Abs(diff.z));
        b.CurrentPart.transform.localRotation = b.Direction.localRotation;//Quaternion.Euler(diff);
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
