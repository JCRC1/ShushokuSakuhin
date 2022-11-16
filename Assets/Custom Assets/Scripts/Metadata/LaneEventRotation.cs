using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LaneEventRotation : LaneEvent
{
    // Float since the only axis to rotate from is the z axis
    public float targetRotation;
    public bool pivotAtEnd;
}
