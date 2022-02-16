using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SelectedEventEditor : MonoBehaviour
{
    public MoveEventHolder m_selectedMovementEvent;
    public RotEventHolder m_selectedRotationEvent;
    public FadeEventHolder m_selectedFadeEvent;
    public LengthEventHolder m_selectedLengthEvent;

    public void SelectMoveEvent(MoveEventHolder _holder)
    {
        m_selectedMovementEvent = _holder;
        m_selectedRotationEvent = null;
        m_selectedFadeEvent = null;
        m_selectedLengthEvent = null;
    }

    public void SelectRotEvent(RotEventHolder _holder)
    {
        m_selectedMovementEvent = null;
        m_selectedRotationEvent = _holder;
        m_selectedFadeEvent = null;
        m_selectedLengthEvent = null;
    }

    public void SelectFadeEvent(FadeEventHolder _holder)
    {
        m_selectedMovementEvent = null;
        m_selectedRotationEvent = null;
        m_selectedFadeEvent = _holder;
        m_selectedLengthEvent = null;
    }

    public void SelectLengthEvent(LengthEventHolder _holder)
    {
        m_selectedMovementEvent = null;
        m_selectedRotationEvent = null;
        m_selectedFadeEvent = null;
        m_selectedLengthEvent = _holder;
    }

    public void RemoveFromList()
    {
        if (m_selectedMovementEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.m_chartData;
            LaneEventMovement laneEvent = m_selectedMovementEvent.m_heldLaneEvent;
            int movIndex = m_selectedMovementEvent.m_indexOfThis;
            int lane = m_selectedMovementEvent.m_laneID;
            GameObject item = EventListDisplay.Instance.m_movements[lane].m_objects[movIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.m_lanes[lane];

            // Return corresponding lane back to previous position if we are in front
            if (movIndex == chart.m_lane[lane].m_laneEventsMovement.Count - 1 && chart.m_lane[lane].m_laneEventsMovement.Count - 1 > 0)
            {
                correspondingLane.transform.position = chart.m_lane[lane].m_laneEventsMovement[movIndex - 1].m_targetPosition;

                correspondingLane.GetComponent<LaneHandler>().m_laneEventMovement = chart.m_lane[lane].m_laneEventsMovement[movIndex - 1];
            }
            else if (LevelEditorManager.Instance.m_currentMovementIndex[lane] == 0 && movIndex == 0)
            {
                correspondingLane.transform.position = chart.m_lane[lane].m_initialPosition;

                LaneEventMovement origin = new LaneEventMovement();
                origin.m_beat = 0.0f;
                origin.m_duration = 0.0f;
                origin.m_easeType = 0.0f;
                origin.m_targetPosition = chart.m_lane[lane].m_initialPosition;

                correspondingLane.GetComponent<LaneHandler>().m_laneEventMovement = origin;
                correspondingLane.GetComponent<LaneHandler>().m_movementStartPosition = chart.m_lane[lane].m_initialPosition;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.m_currentMovementIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.m_movements[lane].m_moves.Remove(laneEvent);
            EventListDisplay.Instance.m_movements[lane].m_objects.Remove(item);
            chart.m_lane[lane].m_laneEventsMovement.Remove(laneEvent);
            Destroy(item);
        }
        else if (m_selectedRotationEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.m_chartData;
            LaneEventRotation laneEvent = m_selectedRotationEvent.m_heldLaneEvent;
            int rotIndex = m_selectedRotationEvent.m_indexOfThis;
            int lane = m_selectedRotationEvent.m_laneID;
            GameObject item = EventListDisplay.Instance.m_rotations[lane].m_objects[rotIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.m_lanes[lane];

            // Return corresponding lane back to previous position
            if (rotIndex == chart.m_lane[lane].m_laneEventsRotation.Count - 1 && chart.m_lane[lane].m_laneEventsRotation.Count - 1 > 0)
            {
                correspondingLane.transform.rotation = Quaternion.Euler(0.0f, 0.0f, chart.m_lane[lane].m_laneEventsRotation[rotIndex - 1].m_targetRotation);
                correspondingLane.GetComponent<LaneHandler>().m_laneEventRotation = chart.m_lane[lane].m_laneEventsRotation[rotIndex - 1];
            }
            else if (LevelEditorManager.Instance.m_currentRotationIndex[lane] == 0 && rotIndex == 0)
            {
                correspondingLane.transform.rotation = Quaternion.Euler(0.0f, 0.0f, chart.m_lane[lane].m_initialRotation);

                LaneEventRotation origin = new LaneEventRotation();
                origin.m_beat = 0.0f;
                origin.m_duration = 0.0f;
                origin.m_easeType = 0.0f;
                origin.m_targetRotation = chart.m_lane[lane].m_initialRotation;

                correspondingLane.GetComponent<LaneHandler>().m_laneEventRotation = origin;
                correspondingLane.GetComponent<LaneHandler>().m_startRotation = chart.m_lane[lane].m_initialRotation;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.m_currentRotationIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.m_rotations[lane].m_rots.Remove(laneEvent);
            EventListDisplay.Instance.m_rotations[lane].m_objects.Remove(item);
            chart.m_lane[lane].m_laneEventsRotation.Remove(laneEvent);
            Destroy(item);
        }
        else if (m_selectedFadeEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.m_chartData;
            LaneEventFade laneEvent = m_selectedFadeEvent.m_heldLaneEvent;
            int fadeIndex = m_selectedFadeEvent.m_indexOfThis;
            int lane = m_selectedFadeEvent.m_laneID;
            GameObject item = EventListDisplay.Instance.m_fades[lane].m_objects[fadeIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.m_lanes[lane];

            // Return corresponding lane back to previous position
            if (fadeIndex == chart.m_lane[lane].m_laneEventFade.Count - 1 && chart.m_lane[lane].m_laneEventFade.Count - 1 > 0)
            {
                correspondingLane.GetComponent<LineRenderer>().startColor = new Color(correspondingLane.GetComponent<LineRenderer>().startColor.r, correspondingLane.GetComponent<LineRenderer>().startColor.g, correspondingLane.GetComponent<LineRenderer>().startColor.b, chart.m_lane[lane].m_laneEventFade[fadeIndex - 1].m_targetAlpha);
                correspondingLane.GetComponent<LineRenderer>().endColor = new Color(correspondingLane.GetComponent<LineRenderer>().endColor.r, correspondingLane.GetComponent<LineRenderer>().endColor.g, correspondingLane.GetComponent<LineRenderer>().endColor.b, chart.m_lane[lane].m_laneEventFade[fadeIndex - 1].m_targetAlpha);

                correspondingLane.GetComponent<LaneHandler>().m_laneEventFade = chart.m_lane[lane].m_laneEventFade[fadeIndex - 1];
            }
            else if (LevelEditorManager.Instance.m_currentFadeIndex[lane] == 0 && fadeIndex == 0)
            {
                correspondingLane.GetComponent<LineRenderer>().startColor = new Color(correspondingLane.GetComponent<LineRenderer>().startColor.r, correspondingLane.GetComponent<LineRenderer>().startColor.g, correspondingLane.GetComponent<LineRenderer>().startColor.b, chart.m_lane[lane].m_initialAlpha);
                correspondingLane.GetComponent<LineRenderer>().endColor = new Color(correspondingLane.GetComponent<LineRenderer>().endColor.r, correspondingLane.GetComponent<LineRenderer>().endColor.g, correspondingLane.GetComponent<LineRenderer>().endColor.b, chart.m_lane[lane].m_initialAlpha);

                LaneEventFade origin = new LaneEventFade();
                origin.m_beat = 0.0f;
                origin.m_duration = 0.0f;
                origin.m_easeType = 0.0f;
                origin.m_targetAlpha = chart.m_lane[lane].m_initialAlpha;

                correspondingLane.GetComponent<LaneHandler>().m_laneEventFade = origin;
                correspondingLane.GetComponent<LaneHandler>().m_startAlpha = chart.m_lane[lane].m_initialAlpha;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.m_currentFadeIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.m_fades[lane].m_fades.Remove(laneEvent);
            EventListDisplay.Instance.m_fades[lane].m_objects.Remove(item);
            chart.m_lane[lane].m_laneEventFade.Remove(laneEvent);
            Destroy(item);
        }
        else if (m_selectedLengthEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.m_chartData;
            LaneEventLength laneEvent = m_selectedLengthEvent.m_heldLaneEvent;
            int lengthIndex = m_selectedLengthEvent.m_indexOfThis;
            int lane = m_selectedLengthEvent.m_laneID;
            GameObject item = EventListDisplay.Instance.m_lengths[lane].m_objects[lengthIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.m_lanes[lane];

            // Return corresponding lane back to previous position
            if (lengthIndex == chart.m_lane[lane].m_laneEventFade.Count - 1 && chart.m_lane[lane].m_laneEventFade.Count - 1 > 0)
            {
                correspondingLane.transform.GetChild(0).localPosition = new Vector2(chart.m_lane[lane].m_laneEventLength[lengthIndex - 1].m_targetLength, 0);
                correspondingLane.GetComponent<LaneHandler>().m_laneEventLength = chart.m_lane[lane].m_laneEventLength[lengthIndex - 1];
            }
            else if (LevelEditorManager.Instance.m_currentLengthIndex[lane] == 0 && lengthIndex == 0)
            {
                correspondingLane.transform.GetChild(0).localPosition = new Vector2(chart.m_lane[lane].m_initialLength, 0);

                LaneEventLength origin = new LaneEventLength();
                origin.m_beat = 0.0f;
                origin.m_duration = 0.0f;
                origin.m_easeType = 0;
                origin.m_targetLength = chart.m_lane[lane].m_initialLength;

                correspondingLane.GetComponent<LaneHandler>().m_laneEventLength = origin;
                correspondingLane.GetComponent<LaneHandler>().m_startLength = chart.m_lane[lane].m_initialLength;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.m_currentLengthIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.m_lengths[lane].m_lengths.Remove(laneEvent);
            EventListDisplay.Instance.m_lengths[lane].m_objects.Remove(item);
            chart.m_lane[lane].m_laneEventLength.Remove(laneEvent);
            Destroy(item);
        }
    }

    public void EditMoveEvent(GameObject _display)
    {
        if (m_selectedMovementEvent)
        {
            _display.SetActive(true);           
        }
    }

    public void EditRotEvent(GameObject _display)
    {
        if (m_selectedRotationEvent)
        {
            _display.SetActive(true);
        }
    }

    public void EditFadeEvent(GameObject _display)
    {
        if (m_selectedFadeEvent)
        {
            _display.SetActive(true);
        }
    }

    public void EditLengthEvent(GameObject _display)
    {
        if (m_selectedLengthEvent)
        {
            _display.SetActive(true);
        }
    }

    public void SeekToSelectedEvent()
    {
        if (m_selectedMovementEvent)
        {
            LevelEditorManager.Instance.m_audioSource.time = (LevelEditorManager.Instance.m_secPerBeat * m_selectedMovementEvent.m_heldLaneEvent.m_beat) + LevelEditorManager.Instance.m_chartData.m_trackOffset;
        }
        else if (m_selectedRotationEvent)
        {
            LevelEditorManager.Instance.m_audioSource.time = (LevelEditorManager.Instance.m_secPerBeat * m_selectedRotationEvent.m_heldLaneEvent.m_beat) + LevelEditorManager.Instance.m_chartData.m_trackOffset;
        }
        else if (m_selectedFadeEvent)
        {
            LevelEditorManager.Instance.m_audioSource.time = (LevelEditorManager.Instance.m_secPerBeat * m_selectedFadeEvent.m_heldLaneEvent.m_beat) + LevelEditorManager.Instance.m_chartData.m_trackOffset;
        }
        else if (m_selectedLengthEvent)
        {
            LevelEditorManager.Instance.m_audioSource.time = (LevelEditorManager.Instance.m_secPerBeat * m_selectedLengthEvent.m_heldLaneEvent.m_beat) + LevelEditorManager.Instance.m_chartData.m_trackOffset;
        }
    }
}
