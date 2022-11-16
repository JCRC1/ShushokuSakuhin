using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEventHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int laneID;
    public int indexOfThis;
    public LaneEventFade heldLaneEvent;

    private void Update()
    {
        indexOfThis = EventListDisplay.Instance.fades[laneID].fades.IndexOf(heldLaneEvent);
    }
}
