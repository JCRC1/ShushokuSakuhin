using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LaneEventFade : LaneEvent
{
    public float targetAlpha = 1.0f;
    public bool fadeNotes;
}
