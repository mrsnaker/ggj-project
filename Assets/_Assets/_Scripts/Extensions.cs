using System;
using System.Collections.Generic;
using DNA;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GameState
{
    PlayStageA,
    PlayStageB,
    CalculateResult,
    LoadingNextLevel,
}

[Serializable]
public class CompareSlotDNA
{
    [SerializeField] private int _idPartHuman;
    [Range(0,100)] [SerializeField] private float _rotateAngle;
    public int IdPartHuman => _idPartHuman;
    public float RotateAngle => _rotateAngle;
}

public static class Extension
{
    public static float DistanceXTo(this Transform fromTransform, Transform toTransform) =>
        Mathf.Abs(fromTransform.position.x - toTransform.position.x);

    public static void ChangeDNASlots(this Slot slot, Slot newSlot, bool fast = false)
    {
        var tempSlotDNA = slot.DNA;
        slot.DNA = newSlot.DNA;
        newSlot.DNA = tempSlotDNA;
        slot.DNA.Slot = slot;
        newSlot.DNA.Slot = newSlot;
        slot.DNA.ResetSmoothPos(fast);
        newSlot.DNA.ResetSmoothPos(fast);

        if(!Application.isPlaying) return;
        var slot1 = Human.GetCurrentSlotFromPart(Human.HumanParts[slot.DNA.IDPartHuman]);
        var slot2 = Human.GetCurrentSlotFromPart(Human.HumanParts[newSlot.DNA.IDPartHuman]);
        Human.SwapParts(slot1, slot2);
    }

    public static bool CheckBorderViewForDNA(this Transform transform, float dir, Vector3 newPos)
    {
        if ((Screen.width * 0.9f < GameManager.DNACamera.WorldToScreenPoint(transform.position).x))
            if(dir >= 0 || GameManager.DNACamera.WorldToScreenPoint(newPos).x > Screen.width * 0.9f) return false;

        if (Screen.width * 0.1f > GameManager.DNACamera.WorldToScreenPoint(transform.position).x)
            if(dir <= 0 || GameManager.DNACamera.WorldToScreenPoint(newPos).x < Screen.width * 0.1f) return false;
        return true;
    }
    
    public static void Shuffle<T>(this IList<T> data)
    {
        for (var i = 0; i < data.Count; i++)
        {
            var tmp = data[i];
            var random = Random.Range(0, data.Count);
            data[i] = data[random];
            data[random] = tmp;
        }
    }
}