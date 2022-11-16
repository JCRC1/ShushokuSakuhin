using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotEventHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int laneID;
    public int indexOfThis;
    public LaneEventRotation heldLaneEvent;

    private void Update()
    {
        indexOfThis = EventListDisplay.Instance.rotations[laneID].rots.IndexOf(heldLaneEvent);
    }
}
