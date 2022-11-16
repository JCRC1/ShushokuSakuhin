using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SelectedEventEditor : MonoBehaviour
{
    public MoveEventHolder selectedMovementEvent;
    public RotEventHolder selectedRotationEvent;
    public FadeEventHolder selectedFadeEvent;
    public LengthEventHolder selectedLengthEvent;

    public void SelectMoveEvent(MoveEventHolder _holder)
    {
        selectedMovementEvent = _holder;
        selectedRotationEvent = null;
        selectedFadeEvent = null;
        selectedLengthEvent = null;
    }

    public void SelectRotEvent(RotEventHolder _holder)
    {
        selectedMovementEvent = null;
        selectedRotationEvent = _holder;
        selectedFadeEvent = null;
        selectedLengthEvent = null;
    }

    public void SelectFadeEvent(FadeEventHolder _holder)
    {
        selectedMovementEvent = null;
        selectedRotationEvent = null;
        selectedFadeEvent = _holder;
        selectedLengthEvent = null;
    }

    public void SelectLengthEvent(LengthEventHolder _holder)
    {
        selectedMovementEvent = null;
        selectedRotationEvent = null;
        selectedFadeEvent = null;
        selectedLengthEvent = _holder;
    }

    public void RemoveFromList()
    {
        if (selectedMovementEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.chartData;
            LaneEventMovement laneEvent = selectedMovementEvent.heldLaneEvent;
            int movIndex = selectedMovementEvent.indexOfThis;
            int lane = selectedMovementEvent.laneID;
            GameObject item = EventListDisplay.Instance.movements[lane].objects[movIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.lanes[lane];

            // Return corresponding lane back to previous position if we are in front
            if (movIndex == chart.lane[lane].laneEventsMovement.Count - 1 && chart.lane[lane].laneEventsMovement.Count - 1 > 0)
            {
                correspondingLane.transform.position = chart.lane[lane].laneEventsMovement[movIndex - 1].targetPosition;

                correspondingLane.GetComponent<LaneHandler>().laneEventMovement = chart.lane[lane].laneEventsMovement[movIndex - 1];
            }
            else if (LevelEditorManager.Instance.currentMovementIndex[lane] == 0 && movIndex == 0)
            {
                correspondingLane.transform.position = chart.lane[lane].initialPosition;

                LaneEventMovement origin = new LaneEventMovement();
                origin.beat = 0.0f;
                origin.duration = 0.0f;
                origin.easeType = 0.0f;
                origin.targetPosition = chart.lane[lane].initialPosition;

                correspondingLane.GetComponent<LaneHandler>().laneEventMovement = origin;
                correspondingLane.GetComponent<LaneHandler>().movementStartPosition = chart.lane[lane].initialPosition;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.currentMovementIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.movements[lane].moves.Remove(laneEvent);
            EventListDisplay.Instance.movements[lane].objects.Remove(item);
            chart.lane[lane].laneEventsMovement.Remove(laneEvent);
            Destroy(item);
        }
        else if (selectedRotationEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.chartData;
            LaneEventRotation laneEvent = selectedRotationEvent.heldLaneEvent;
            int rotIndex = selectedRotationEvent.indexOfThis;
            int lane = selectedRotationEvent.laneID;
            GameObject item = EventListDisplay.Instance.rotations[lane].objects[rotIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.lanes[lane];

            // Return corresponding lane back to previous position
            if (rotIndex == chart.lane[lane].laneEventsRotation.Count - 1 && chart.lane[lane].laneEventsRotation.Count - 1 > 0)
            {
                correspondingLane.transform.rotation = Quaternion.Euler(0.0f, 0.0f, chart.lane[lane].laneEventsRotation[rotIndex - 1].targetRotation);
                correspondingLane.GetComponent<LaneHandler>().laneEventRotation = chart.lane[lane].laneEventsRotation[rotIndex - 1];
            }
            else if (LevelEditorManager.Instance.currentRotationIndex[lane] == 0 && rotIndex == 0)
            {
                correspondingLane.transform.rotation = Quaternion.Euler(0.0f, 0.0f, chart.lane[lane].initialRotation);

                LaneEventRotation origin = new LaneEventRotation();
                origin.beat = 0.0f;
                origin.duration = 0.0f;
                origin.easeType = 0.0f;
                origin.targetRotation = chart.lane[lane].initialRotation;

                correspondingLane.GetComponent<LaneHandler>().laneEventRotation = origin;
                correspondingLane.GetComponent<LaneHandler>().startRotation = chart.lane[lane].initialRotation;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.currentRotationIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.rotations[lane].rots.Remove(laneEvent);
            EventListDisplay.Instance.rotations[lane].objects.Remove(item);
            chart.lane[lane].laneEventsRotation.Remove(laneEvent);
            Destroy(item);
        }
        else if (selectedFadeEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.chartData;
            LaneEventFade laneEvent = selectedFadeEvent.heldLaneEvent;
            int fadeIndex = selectedFadeEvent.indexOfThis;
            int lane = selectedFadeEvent.laneID;
            GameObject item = EventListDisplay.Instance.fades[lane].objects[fadeIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.lanes[lane];

            // Return corresponding lane back to previous position
            if (fadeIndex == chart.lane[lane].laneEventFade.Count - 1 && chart.lane[lane].laneEventFade.Count - 1 > 0)
            {
                correspondingLane.GetComponent<LineRenderer>().startColor = new Color(correspondingLane.GetComponent<LineRenderer>().startColor.r, correspondingLane.GetComponent<LineRenderer>().startColor.g, correspondingLane.GetComponent<LineRenderer>().startColor.b, chart.lane[lane].laneEventFade[fadeIndex - 1].targetAlpha);
                correspondingLane.GetComponent<LineRenderer>().endColor = new Color(correspondingLane.GetComponent<LineRenderer>().endColor.r, correspondingLane.GetComponent<LineRenderer>().endColor.g, correspondingLane.GetComponent<LineRenderer>().endColor.b, chart.lane[lane].laneEventFade[fadeIndex - 1].targetAlpha);

                correspondingLane.GetComponent<LaneHandler>().laneEventFade = chart.lane[lane].laneEventFade[fadeIndex - 1];
            }
            else if (LevelEditorManager.Instance.currentFadeIndex[lane] == 0 && fadeIndex == 0)
            {
                correspondingLane.GetComponent<LineRenderer>().startColor = new Color(correspondingLane.GetComponent<LineRenderer>().startColor.r, correspondingLane.GetComponent<LineRenderer>().startColor.g, correspondingLane.GetComponent<LineRenderer>().startColor.b, chart.lane[lane].initialAlpha);
                correspondingLane.GetComponent<LineRenderer>().endColor = new Color(correspondingLane.GetComponent<LineRenderer>().endColor.r, correspondingLane.GetComponent<LineRenderer>().endColor.g, correspondingLane.GetComponent<LineRenderer>().endColor.b, chart.lane[lane].initialAlpha);

                LaneEventFade origin = new LaneEventFade();
                origin.beat = 0.0f;
                origin.duration = 0.0f;
                origin.easeType = 0.0f;
                origin.targetAlpha = chart.lane[lane].initialAlpha;

                correspondingLane.GetComponent<LaneHandler>().laneEventFade = origin;
                correspondingLane.GetComponent<LaneHandler>().startAlpha = chart.lane[lane].initialAlpha;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.currentFadeIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.fades[lane].fades.Remove(laneEvent);
            EventListDisplay.Instance.fades[lane].objects.Remove(item);
            chart.lane[lane].laneEventFade.Remove(laneEvent);
            Destroy(item);
        }
        else if (selectedLengthEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.chartData;
            LaneEventLength laneEvent = selectedLengthEvent.heldLaneEvent;
            int lengthIndex = selectedLengthEvent.indexOfThis;
            int lane = selectedLengthEvent.laneID;
            GameObject item = EventListDisplay.Instance.lengths[lane].objects[lengthIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.lanes[lane];

            // Return corresponding lane back to previous position
            if (lengthIndex == chart.lane[lane].laneEventFade.Count - 1 && chart.lane[lane].laneEventFade.Count - 1 > 0)
            {
                correspondingLane.transform.GetChild(0).localPosition = new Vector2(chart.lane[lane].laneEventLength[lengthIndex - 1].targetLength, 0);
                correspondingLane.GetComponent<LaneHandler>().laneEventLength = chart.lane[lane].laneEventLength[lengthIndex - 1];
            }
            else if (LevelEditorManager.Instance.currentLengthIndex[lane] == 0 && lengthIndex == 0)
            {
                correspondingLane.transform.GetChild(0).localPosition = new Vector2(chart.lane[lane].initialLength, 0);

                LaneEventLength origin = new LaneEventLength();
                origin.beat = 0.0f;
                origin.duration = 0.0f;
                origin.easeType = 0;
                origin.targetLength = chart.lane[lane].initialLength;

                correspondingLane.GetComponent<LaneHandler>().laneEventLength = origin;
                correspondingLane.GetComponent<LaneHandler>().startLength = chart.lane[lane].initialLength;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.currentLengthIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.lengths[lane].lengths.Remove(laneEvent);
            EventListDisplay.Instance.lengths[lane].objects.Remove(item);
            chart.lane[lane].laneEventLength.Remove(laneEvent);
            Destroy(item);
        }
    }

    public void EditMoveEvent(GameObject _display)
    {
        if (selectedMovementEvent)
        {
            _display.SetActive(true);           
        }
    }

    public void EditRotEvent(GameObject _display)
    {
        if (selectedRotationEvent)
        {
            _display.SetActive(true);
        }
    }

    public void EditFadeEvent(GameObject _display)
    {
        if (selectedFadeEvent)
        {
            _display.SetActive(true);
        }
    }

    public void EditLengthEvent(GameObject _display)
    {
        if (selectedLengthEvent)
        {
            _display.SetActive(true);
        }
    }

    public void SeekToSelectedEvent()
    {
        if (selectedMovementEvent)
        {
            LevelEditorManager.Instance.audioSource.time = (LevelEditorManager.Instance.secPerBeat * selectedMovementEvent.heldLaneEvent.beat) + LevelEditorManager.Instance.chartData.trackOffset;
        }
        else if (selectedRotationEvent)
        {
            LevelEditorManager.Instance.audioSource.time = (LevelEditorManager.Instance.secPerBeat * selectedRotationEvent.heldLaneEvent.beat) + LevelEditorManager.Instance.chartData.trackOffset;
        }
        else if (selectedFadeEvent)
        {
            LevelEditorManager.Instance.audioSource.time = (LevelEditorManager.Instance.secPerBeat * selectedFadeEvent.heldLaneEvent.beat) + LevelEditorManager.Instance.chartData.trackOffset;
        }
        else if (selectedLengthEvent)
        {
            LevelEditorManager.Instance.audioSource.time = (LevelEditorManager.Instance.secPerBeat * selectedLengthEvent.heldLaneEvent.beat) + LevelEditorManager.Instance.chartData.trackOffset;
        }
    }
}
