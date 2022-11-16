using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LengthEventEditor : MonoBehaviour
{
    public LaneEventLength selectedEvent;

    public InputField eventLane;
    public InputField eventBeat;
    public InputField targetLength;
    public InputField duration;
    public Dropdown easeType;

    public SelectedEventEditor holder;

    public void SetLengthEvent(SelectedEventEditor _holder)
    {
        if (_holder.selectedLengthEvent != null)
        {
            holder = _holder;
            selectedEvent = _holder.selectedLengthEvent.heldLaneEvent;

            eventLane.text = _holder.selectedLengthEvent.laneID.ToString();
            eventBeat.text = _holder.selectedLengthEvent.heldLaneEvent.beat.ToString();
            targetLength.text = _holder.selectedLengthEvent.heldLaneEvent.targetLength.ToString();
            duration.text = _holder.selectedLengthEvent.heldLaneEvent.duration.ToString();
            easeType.value = (int)_holder.selectedLengthEvent.heldLaneEvent.easeType;
        }
    }

    public void ConfirmLengthEdit()
    {
        selectedEvent.beat = float.Parse(eventBeat.text);
        selectedEvent.targetLength = float.Parse(targetLength.text);
        selectedEvent.duration = float.Parse(duration.text);
        selectedEvent.easeType = (LaneEvent.EaseType)easeType.value;

        EventListDisplay.Instance.EditEvent(selectedEvent, holder.selectedLengthEvent);
    }
}
