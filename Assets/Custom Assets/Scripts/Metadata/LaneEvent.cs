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
    public float beat;
    // Duration of the event beat+ duration = new beat to end at
    public float duration;
    // Ease type
    public EaseType easeType;
}
