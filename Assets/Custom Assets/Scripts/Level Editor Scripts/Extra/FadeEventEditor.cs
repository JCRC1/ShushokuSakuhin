using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEventEditor : MonoBehaviour
{
    public LaneEventFade m_selectedEvent;

    public InputField m_eventLane;
    public InputField m_eventBeat;
    public InputField m_targetAlpha;
    public Toggle m_affectingNotes;
    public InputField m_duration;
    public Dropdown m_easeType;

    public void SetFadeEvent(SelectedEventEditor _holder)
    {
        if (_holder.m_selectedFadeEvent != null)
        {
            m_selectedEvent = _holder.m_selectedFadeEvent.m_heldLaneEvent;

            m_eventLane.text = _holder.m_selectedFadeEvent.m_laneID.ToString();
            m_eventBeat.text = _holder.m_selectedFadeEvent.m_heldLaneEvent.m_beat.ToString();
            m_targetAlpha.text = _holder.m_selectedFadeEvent.m_heldLaneEvent.m_targetAlpha.ToString();
            m_affectingNotes.isOn = _holder.m_selectedFadeEvent.m_heldLaneEvent.m_fadeNotes;
            m_duration.text = _holder.m_selectedFadeEvent.m_heldLaneEvent.m_duration.ToString();
            m_easeType.value = (int)_holder.m_selectedFadeEvent.m_heldLaneEvent.m_easeType;
        }
    }

    public void ConfirmFadeEdit()
    {
        m_selectedEvent.m_beat = float.Parse(m_eventBeat.text);
        m_selectedEvent.m_targetAlpha = float.Parse(m_targetAlpha.text);
        m_selectedEvent.m_fadeNotes = m_affectingNotes.isOn;
        m_selectedEvent.m_duration = float.Parse(m_duration.text);
        m_selectedEvent.m_easeType = (LaneEvent.EaseType)m_easeType.value;
    }
}
