using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEventEditor : MonoBehaviour
{
    public LaneEventFade selectedEvent;

    public InputField eventLane;
    public InputField eventBeat;
    public InputField targetAlpha;
    public Toggle affectingNotes;
    public InputField duration;
    public Dropdown easeType;

    public SelectedEventEditor holder;

    public void SetFadeEvent(SelectedEventEditor _holder)
    {
        if (_holder.selectedFadeEvent != null)
        {
            holder = _holder;
            selectedEvent = _holder.selectedFadeEvent.heldLaneEvent;

            eventLane.text = _holder.selectedFadeEvent.laneID.ToString();
            eventBeat.text = _holder.selectedFadeEvent.heldLaneEvent.beat.ToString();
            targetAlpha.text = _holder.selectedFadeEvent.heldLaneEvent.targetAlpha.ToString();
            affectingNotes.isOn = _holder.selectedFadeEvent.heldLaneEvent.fadeNotes;
            duration.text = _holder.selectedFadeEvent.heldLaneEvent.duration.ToString();
            easeType.value = (int)_holder.selectedFadeEvent.heldLaneEvent.easeType;
        }
    }

    public void ConfirmFadeEdit()
    {
        selectedEvent.beat = float.Parse(eventBeat.text);
        selectedEvent.targetAlpha = float.Parse(targetAlpha.text);
        selectedEvent.fadeNotes = affectingNotes.isOn;
        selectedEvent.duration = float.Parse(duration.text);
        selectedEvent.easeType = (LaneEvent.EaseType)easeType.value;

        EventListDisplay.Instance.EditEvent(selectedEvent, holder.selectedFadeEvent);
    }
}
