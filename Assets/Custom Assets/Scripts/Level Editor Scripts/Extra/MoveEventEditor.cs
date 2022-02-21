using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveEventEditor : MonoBehaviour
{
    public LaneEventMovement m_selectedEvent;

    public InputField m_eventLane;
    public InputField m_eventBeat;
    public InputField m_targetX;
    public InputField m_targetY;
    public InputField m_duration;
    public Dropdown m_easeType;

    public SelectedEventEditor m_holder;

    public void SetMoveEvent(SelectedEventEditor _holder)
    {
        if (_holder.m_selectedMovementEvent != null)
        {
            m_holder = _holder;
            m_selectedEvent = _holder.m_selectedMovementEvent.m_heldLaneEvent;

            m_eventLane.text = _holder.m_selectedMovementEvent.m_laneID.ToString();
            m_eventBeat.text = _holder.m_selectedMovementEvent.m_heldLaneEvent.m_beat.ToString();
            m_targetX.text = _holder.m_selectedMovementEvent.m_heldLaneEvent.m_targetPosition.x.ToString();
            m_targetY.text = _holder.m_selectedMovementEvent.m_heldLaneEvent.m_targetPosition.y.ToString();
            m_duration.text = _holder.m_selectedMovementEvent.m_heldLaneEvent.m_duration.ToString();
            m_easeType.value = (int)_holder.m_selectedMovementEvent.m_heldLaneEvent.m_easeType;
        }
    }

    public void ConfirmMoveEdit()
    {
        m_selectedEvent.m_beat = float.Parse(m_eventBeat.text);
        m_selectedEvent.m_targetPosition.x = float.Parse(m_targetX.text);
        m_selectedEvent.m_targetPosition.y = float.Parse(m_targetY.text);
        m_selectedEvent.m_duration = float.Parse(m_duration.text);
        m_selectedEvent.m_easeType = (LaneEvent.EaseType)m_easeType.value;

        EventListDisplay.Instance.EditEvent(m_selectedEvent, m_holder.m_selectedMovementEvent);
    }
}
