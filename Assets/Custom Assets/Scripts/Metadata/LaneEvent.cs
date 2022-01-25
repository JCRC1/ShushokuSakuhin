using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LaneEvent class contains information on lane movement or other such events
/// </summary>
[System.Serializable]
public class LaneEvent
{
    public enum EaseType
    {
        EASE_NONE,
        EASE_NORMAL,
        EASE_IN,
        EASE_OUT
    }


    // When to begin event
    public float m_beat;
    // Duration of the event beat+ duration = new beat to end at
    public float m_duration;
    // Ease type
    public EaseType m_easeType;
}
