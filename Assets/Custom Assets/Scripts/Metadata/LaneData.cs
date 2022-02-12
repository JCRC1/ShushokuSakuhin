using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LaneData class contains all metadata of a lane
/// </summary>
[System.Serializable]
public class LaneData
{
    public Vector2                      m_initialPosition;
    public float                        m_initialRotation;      // Z Axis
    public float                        m_initialAlpha = 1.0f;
    public float                        m_initialLength = 10.0f;

    // Notes on this lane
    public List<SingleNoteData>               m_singleNote;

    // Lane events
    public List<LaneEventMovement>      m_laneEventsMovement;
    public List<LaneEventRotation>      m_laneEventsRotation;
    public List<LaneEventFade>          m_laneEventFade;
    public List<LaneEventLength>        m_laneEventLength;
}
