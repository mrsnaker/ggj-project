using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using DNA;

public class Human : MonoBehaviour
{
    private static Human _instance;
    private static Human Instance => _instance ? _instance : _instance = FindObjectOfType<Human>();

    [SerializeField] private HumanPartSlot _humanPartSlotPrefab;
    [SerializeField] private List<HumanPart> _humanParts;
    public static List<HumanPart> HumanParts => Instance._humanParts;
    [SerializeField] private Dictionary<HumanPartSize, List<HumanPartSlot>> _humanSlots = new Dictionary<HumanPartSize, List<HumanPartSlot>>();
    public static Dictionary<HumanPartSize, List<HumanPartSlot>> HumanSlots => Instance._humanSlots;

    [SerializeField] private SkinnedMeshRenderer _chest;

    private static System.Random rng = new System.Random();
    private Sequence _chestAnimation;
    public static Transform HumanTransform => Instance.transform;

    private int _currentPart1 = -1;
    private int _currentPart2 = -1;

    private void Awake()
    {
        _chestAnimation = StartChestAnimation();

        for (int i = 0; i < _humanParts.Count; i++)
        {
            var humanPart = _humanParts[i];
            humanPart.ID = i;
            humanPart.OriginalPos = humanPart.transform.localPosition;
            humanPart.OriginalRotation = humanPart.transform.localRotation;
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

        for (int i = 0; i < DNAManager.DNAList.Count; i++)
        {
            //HumanParts[DNAManager.DNAList[i].IDPartHuman]
        }

        var randomSlot1 = Random.Range(0, _humanSlots[HumanPartSize.Big].Count);
        var randomSlot2 = (randomSlot1 + Random.Range(1, _humanSlots[HumanPartSize.Big].Count)) % _humanSlots[HumanPartSize.Big].Count;
        //Debug.Log(randomSlot1 + " " + randomSlot2);
        //SwapParts(_humanSlots[HumanPartSize.Big][randomSlot1], _humanSlots[HumanPartSize.Big][randomSlot2]);
        //SwapParts(_humanSlots[HumanPartSize.Big][1], _humanSlots[HumanPartSize.Big][2]);
        //ShuffleParts(_humanPartSlots);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(_currentPart1 !=-1 && _currentPart2!=-1)
            {
                SwapParts(_humanSlots[HumanPartSize.Big][_currentPart1], _humanSlots[HumanPartSize.Big][_currentPart2]);
            }
            _currentPart1 = Random.Range(0, _humanSlots[HumanPartSize.Big].Count);
            _currentPart2 = (_currentPart1 + Random.Range(1, _humanSlots[HumanPartSize.Big].Count)) % _humanSlots[HumanPartSize.Big].Count;
            //Debug.Log(_currentPart1 + " " + _currentPart2);
            _currentPart1 = 0;
            _currentPart2 = 2;
            SwapParts(_humanSlots[HumanPartSize.Big][_currentPart1], _humanSlots[HumanPartSize.Big][_currentPart2]);
        }
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
        _chestAnimation.Complete();
    }

    private static HumanPartSlot GetRightSlotFromPart(HumanPart part)
    {
        return Instance._humanSlots[part.Size].Find(p => p.ID == part.ID);
    }

    public static HumanPartSlot GetCurrentSlotFromPart(HumanPart part)
    {
        return HumanSlots[part.Size].Find(ps => ps.CurrentPart.Equals(part));
    }

    private static HumanPart GetRightPartFromSlot(HumanPartSlot partSlot)
    {
        return Instance._humanParts.Find(p => p.ID == partSlot.ID);
    }

    private static HumanPart GetCurrentPartFromSlot(HumanPartSlot partSlot)
    {
        return Instance._humanParts.Find(p => p == partSlot.CurrentPart);
    }

    public static void SwapParts(HumanPartSlot a, HumanPartSlot b)
    {
        //Debug.Log(a.name + "--" + b.name);
        if (a.Size != b.Size) return;
        if (a.ID == b.ID) return;
        var temp = a.CurrentPart;
        a.CurrentPart = b.CurrentPart;
        a.CurrentPart.transform.parent = a.transform;
        a.CurrentPart.transform.localPosition = Vector3.zero;
        a.CurrentPart.transform.localRotation = Quaternion.identity;
        b.CurrentPart.transform.parent = a.transform;

        if (a.CurrentPart.ID == a.ID)
        {
            a.CurrentPart.transform.localPosition = Vector3.zero;
            a.CurrentPart.transform.localRotation = Quaternion.identity;
        }
        else
        {
            //if(a.CurrentPart.Direction.localRotation.y <= 90)
            //    a.CurrentPart.transform.localRotation = a.Direction.localRotation;
            //else a.CurrentPart.transform.localRotation = Quaternion.Inverse(a.Direction.localRotation);
            a.CurrentPart.transform.localRotation = Quaternion.FromToRotation(a.CurrentPart.transform.localRotation.eulerAngles, a.Direction.localRotation.eulerAngles);
        }
        
        b.CurrentPart = temp;
        b.CurrentPart.transform.parent = b.transform;
        b.CurrentPart.transform.localPosition = Vector3.zero;
        b.CurrentPart.transform.localRotation = Quaternion.identity;

        if (b.CurrentPart.ID == b.ID)
        {
            b.CurrentPart.transform.localPosition = Vector3.zero;
            b.CurrentPart.transform.localRotation = Quaternion.identity;
        }
        else
        {

            b.CurrentPart.transform.localRotation = b.Direction.localRotation;
            b.CurrentPart.transform.localRotation = Quaternion.FromToRotation(b.CurrentPart.transform.localRotation.eulerAngles, b.Direction.localRotation.eulerAngles);
        }
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
    
    #if UNITY_EDITOR

    public void SetHumanParts()
    {
        _humanParts = new List<HumanPart>();
        _humanParts = GetComponentsInChildren<HumanPart>().ToList();
    }

#endif
}
