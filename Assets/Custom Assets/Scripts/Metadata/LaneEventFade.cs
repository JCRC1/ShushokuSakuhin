using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LaneEventFade : LaneEvent
{
    public float m_targetAlpha = 1.0f;
    public bool m_fadeNotes;
}
