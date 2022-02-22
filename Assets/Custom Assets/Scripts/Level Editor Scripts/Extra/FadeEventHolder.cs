using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEventHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int m_laneID;
    public int m_indexOfThis;
    public LaneEventFade m_heldLaneEvent;

    private void Update()
    {
        m_indexOfThis = EventListDisplay.Instance.m_fades[m_laneID].m_fades.IndexOf(m_heldLaneEvent);
    }
}
