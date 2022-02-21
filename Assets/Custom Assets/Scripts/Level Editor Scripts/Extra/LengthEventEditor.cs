using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LengthEventEditor : MonoBehaviour
{
    public LaneEventLength m_selectedEvent;

    public InputField m_eventLane;
    public InputField m_eventBeat;
    public InputField m_targetLength;
    public InputField m_duration;
    public Dropdown m_easeType;

    public SelectedEventEditor m_holder;

    public void SetLengthEvent(SelectedEventEditor _holder)
    {
        if (_holder.m_selectedLengthEvent != null)
        {
            m_holder = _holder;
            m_selectedEvent = _holder.m_selectedLengthEvent.m_heldLaneEvent;

            m_eventLane.text = _holder.m_selectedLengthEvent.m_laneID.ToString();
            m_eventBeat.text = _holder.m_selectedLengthEvent.m_heldLaneEvent.m_beat.ToString();
            m_targetLength.text = _holder.m_selectedLengthEvent.m_heldLaneEvent.m_targetLength.ToString();
            m_duration.text = _holder.m_selectedLengthEvent.m_heldLaneEvent.m_duration.ToString();
            m_easeType.value = (int)_holder.m_selectedLengthEvent.m_heldLaneEvent.m_easeType;
        }
    }

    public void ConfirmLengthEdit()
    {
        m_selectedEvent.m_beat = float.Parse(m_eventBeat.text);
        m_selectedEvent.m_targetLength = float.Parse(m_targetLength.text);
        m_selectedEvent.m_duration = float.Parse(m_duration.text);
        m_selectedEvent.m_easeType = (LaneEvent.EaseType)m_easeType.value;

        EventListDisplay.Instance.EditEvent(m_selectedEvent, m_holder.m_selectedLengthEvent);
    }
}
