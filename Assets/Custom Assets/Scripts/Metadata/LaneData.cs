using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LaneData class contains all metadata of a lane
/// </summary>
[System.Serializable]
public class LaneData
{
    public Vector2                      initialPosition;
    public float                        initialRotation;      // Z Axis
    public float                        initialAlpha = 1.0f;
    public float                        initialLength = 10.0f;

    // Notes on this lane
    public List<SingleNoteData>               singleNote;
    public List<HoldNoteData>                 holdNote;

    // Lane events
    public List<LaneEventMovement>      laneEventsMovement;
    public List<LaneEventRotation>      laneEventsRotation;
    public List<LaneEventFade>          laneEventFade;
    public List<LaneEventLength>        laneEventLength;
}
