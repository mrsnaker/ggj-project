using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPart : MonoBehaviour
{
    [SerializeField] private HumanPartSize _size;
    [SerializeField] private int _id;
    [SerializeField] private Transform _direction;
    public int ID => _id;
    public HumanPartSize Size => _size;
    public Transform Direction => _direction;
}

public enum HumanPartSize
{
    Small,
    Medium,
    Big
}
