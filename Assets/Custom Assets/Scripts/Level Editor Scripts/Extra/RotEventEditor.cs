using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotEventEditor : MonoBehaviour
{
    public LaneEventRotation m_selectedEvent;

    public InputField m_eventLane;
    public InputField m_eventBeat;
    public InputField m_targetRotation;
    public InputField m_duration;
    public Dropdown m_easeType;

    public SelectedEventEditor m_holder;

    public void SetRotEvent(SelectedEventEditor _holder)
    {
        if (_holder.m_selectedRotationEvent != null)
        {
            m_holder = _holder;
            m_selectedEvent = _holder.m_selectedRotationEvent.m_heldLaneEvent;

            m_eventLane.text = _holder.m_selectedRotationEvent.m_laneID.ToString();
            m_eventBeat.text = _holder.m_selectedRotationEvent.m_heldLaneEvent.m_beat.ToString();
            m_targetRotation.text = _holder.m_selectedRotationEvent.m_heldLaneEvent.m_targetRotation.ToString();
            m_duration.text = _holder.m_selectedRotationEvent.m_heldLaneEvent.m_duration.ToString();
            m_easeType.value = (int)_holder.m_selectedRotationEvent.m_heldLaneEvent.m_easeType; 
        }
    }

    public void ConfirmRotEdit()
    {
        m_selectedEvent.m_beat = float.Parse(m_eventBeat.text);
        m_selectedEvent.m_targetRotation = float.Parse(m_targetRotation.text);
        m_selectedEvent.m_duration = float.Parse(m_duration.text);
        m_selectedEvent.m_easeType = (LaneEvent.EaseType)m_easeType.value;

        EventListDisplay.Instance.EditEvent(m_selectedEvent, m_holder.m_selectedRotationEvent);
    }
}
