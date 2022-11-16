using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotEventEditor : MonoBehaviour
{
    public LaneEventRotation selectedEvent;

    public InputField eventLane;
    public InputField eventBeat;
    public InputField targetRotation;
    public InputField duration;
    public Dropdown easeType;

    public SelectedEventEditor holder;

    public void SetRotEvent(SelectedEventEditor _holder)
    {
        if (_holder.selectedRotationEvent != null)
        {
            holder = _holder;
            selectedEvent = _holder.selectedRotationEvent.heldLaneEvent;

            eventLane.text = _holder.selectedRotationEvent.laneID.ToString();
            eventBeat.text = _holder.selectedRotationEvent.heldLaneEvent.beat.ToString();
            targetRotation.text = _holder.selectedRotationEvent.heldLaneEvent.targetRotation.ToString();
            duration.text = _holder.selectedRotationEvent.heldLaneEvent.duration.ToString();
            easeType.value = (int)_holder.selectedRotationEvent.heldLaneEvent.easeType; 
        }
    }

    public void ConfirmRotEdit()
    {
        selectedEvent.beat = float.Parse(eventBeat.text);
        selectedEvent.targetRotation = float.Parse(targetRotation.text);
        selectedEvent.duration = float.Parse(duration.text);
        selectedEvent.easeType = (LaneEvent.EaseType)easeType.value;

        EventListDisplay.Instance.EditEvent(selectedEvent, holder.selectedRotationEvent);
    }
}
