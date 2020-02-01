using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DNA
{
    public class Slot : MonoBehaviour
    {
        private DNA _dna;
        public DNA DNA
        {
            get => _dna;
            set => _dna = value;
        }

        public Vector3 Pos => transform.position;
    }
}