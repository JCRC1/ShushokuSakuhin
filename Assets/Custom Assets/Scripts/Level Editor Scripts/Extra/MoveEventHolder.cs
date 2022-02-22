using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEventHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int m_laneID;
    public int m_indexOfThis;
    public LaneEventMovement m_heldLaneEvent;

    private void Update()
    {
        m_indexOfThis = EventListDisplay.Instance.m_movements[m_laneID].m_moves.IndexOf(m_heldLaneEvent);
    }
}
