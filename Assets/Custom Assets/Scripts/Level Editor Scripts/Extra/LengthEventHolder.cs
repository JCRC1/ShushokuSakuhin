using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthEventHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int m_laneID;
    public int m_indexOfThis;
    public LaneEventLength m_heldLaneEvent;

    private void Update()
    {
        m_indexOfThis = EventListDisplay.Instance.m_lengths[m_laneID].m_lengths.IndexOf(m_heldLaneEvent);
    }
}
