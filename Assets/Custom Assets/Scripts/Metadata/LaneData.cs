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

    // Notes on this lane
    public List<NoteData>               m_notes;

    // Lane events
    public List<LaneEventMovement>      m_laneEventsMovement;
    public List<LaneEventRotation>      m_laneEventsRotation;
}
