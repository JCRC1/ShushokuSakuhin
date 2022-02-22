using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotEventHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int m_laneID;
    public int m_indexOfThis;
    public LaneEventRotation m_heldLaneEvent;

    private void Update()
    {
        m_indexOfThis = EventListDisplay.Instance.m_rotations[m_laneID].m_rots.IndexOf(m_heldLaneEvent);
    }
}
