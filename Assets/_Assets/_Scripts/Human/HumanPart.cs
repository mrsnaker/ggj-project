using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPart : MonoBehaviour
{
    [SerializeField] private HumanPartSize _size;
    [SerializeField] private int _id;
    [SerializeField] private Transform _direction;

    private Vector3 _originalPos;
    private Quaternion _originalRotation;
    public int ID
    {
        get => _id;
        set => _id = value;
    }
    public HumanPartSize Size => _size;
    public Transform Direction => _direction ? _direction : transform.Find("Direction");

    public Vector3 OriginalPos
    {
        get=> _originalPos;
        set => _originalPos = value;
    }

    public Quaternion OriginalRotation
    {
        get => _originalRotation;
        set => _originalRotation = value;
    }
}

public enum HumanPartSize
{
    Small,
    Medium,
    Big
}
