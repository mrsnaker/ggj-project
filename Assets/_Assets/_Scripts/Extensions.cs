using DNA;
using UnityEngine;

public enum GameState
{
    PlayStage,
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

    public static bool CheckBorderViewForDNA(this Transform transform, float dir, Vector3 newPos)
    {
        if ((Screen.width * 0.9f < GameManager.DNACamera.WorldToScreenPoint(transform.position).x))
            if(dir >= 0 || GameManager.DNACamera.WorldToScreenPoint(newPos).x > Screen.width * 0.9f) return false;

        if (Screen.width * 0.1f > GameManager.DNACamera.WorldToScreenPoint(transform.position).x)
            if(dir <= 0 || GameManager.DNACamera.WorldToScreenPoint(newPos).x < Screen.width * 0.1f) return false;
        return true;
    }
}