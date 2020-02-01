using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPartSlot : MonoBehaviour
{
    [SerializeField] private Transform _direction;
    public int ID { get; set; }
    public HumanPartSize Size { get; set; }

    public HumanPart CurrentPart { get; set; }
    public Transform Direction=> _direction;

    public bool IsPartRight => CurrentPart.ID == ID;
}
