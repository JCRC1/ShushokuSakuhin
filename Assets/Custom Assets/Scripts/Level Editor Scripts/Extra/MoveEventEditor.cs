using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveEventEditor : MonoBehaviour
{
    public LaneEventMovement selectedEvent;

    public InputField eventLane;
    public InputField eventBeat;
    public InputField targetX;
    public InputField targetY;
    public InputField duration;
    public Dropdown easeType;

    public SelectedEventEditor holder;

    public void SetMoveEvent(SelectedEventEditor _holder)
    {
        if (_holder.selectedMovementEvent != null)
        {
            holder = _holder;
            selectedEvent = _holder.selectedMovementEvent.heldLaneEvent;

            eventLane.text = _holder.selectedMovementEvent.laneID.ToString();
            eventBeat.text = _holder.selectedMovementEvent.heldLaneEvent.beat.ToString();
            targetX.text = _holder.selectedMovementEvent.heldLaneEvent.targetPosition.x.ToString();
            targetY.text = _holder.selectedMovementEvent.heldLaneEvent.targetPosition.y.ToString();
            duration.text = _holder.selectedMovementEvent.heldLaneEvent.duration.ToString();
            easeType.value = (int)_holder.selectedMovementEvent.heldLaneEvent.easeType;
        }
    }

    public void ConfirmMoveEdit()
    {
        selectedEvent.beat = float.Parse(eventBeat.text);
        selectedEvent.targetPosition.x = float.Parse(targetX.text);
        selectedEvent.targetPosition.y = float.Parse(targetY.text);
        selectedEvent.duration = float.Parse(duration.text);
        selectedEvent.easeType = (LaneEvent.EaseType)easeType.value;

        EventListDisplay.Instance.EditEvent(selectedEvent, holder.selectedMovementEvent);
    }
}
