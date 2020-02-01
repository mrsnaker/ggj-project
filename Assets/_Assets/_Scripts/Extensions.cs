using DNA;
using UnityEngine;

public enum GameState
{
    FirstStage,
    SecondStage,
    CalculateResult,
    LoadingNextLevel,
}

public static class Extension
{
    public static float DistanceXTo(this Transform fromTransform, Transform toTransform) =>
        Mathf.Abs(fromTransform.position.x - toTransform.position.x);

    public static void ChangeDNASlots(this Slot slot, Slot newSlot)
    {
        var tempSlotDNA = slot.DNA;
        slot.DNA = newSlot.DNA;
        newSlot.DNA = tempSlotDNA;
        slot.DNA.Slot = slot;
        newSlot.DNA.Slot = newSlot;
        slot.DNA.ResetSmoothPos();
        newSlot.DNA.ResetSmoothPos();
    }

    public static bool CheckBorderViewForDNA(this Transform transform, int dir)
    {
        if ((Screen.width - 15f) < GameManager.DNACamera.WorldToScreenPoint(transform.position).x) return false;
        return (15f < GameManager.DNACamera.WorldToScreenPoint(transform.position).x);
    }
}