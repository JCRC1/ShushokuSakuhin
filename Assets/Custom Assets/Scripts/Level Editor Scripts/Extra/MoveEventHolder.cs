using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEventHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int laneID;
    public int indexOfThis;
    public LaneEventMovement heldLaneEvent;

    private void Update()
    {
        indexOfThis = EventListDisplay.Instance.movements[laneID].moves.IndexOf(heldLaneEvent);
    }
}
