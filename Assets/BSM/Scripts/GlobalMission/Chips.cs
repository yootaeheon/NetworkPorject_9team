using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chips : MonoBehaviour
{
    [field:SerializeField] public ChipType _chipType { get; set; }

    [field: HideInInspector] public bool IsMove { get; set; }

}
