using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthEventHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int laneID;
    public int indexOfThis;
    public LaneEventLength heldLaneEvent;

    private void Update()
    {
        indexOfThis = EventListDisplay.Instance.lengths[laneID].lengths.IndexOf(heldLaneEvent);
    }
}
