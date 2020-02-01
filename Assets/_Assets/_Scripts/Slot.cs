using UnityEngine;

namespace DNA
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private int _id;
        public int ID
        {
            get => _id;
            set => _id = value;
        }

        [SerializeField] private DNA _dna;
        public DNA DNA
        {
            get => _dna;
            set => _dna = value;
        }

        public Vector3 Pos => transform.position;
    }
}