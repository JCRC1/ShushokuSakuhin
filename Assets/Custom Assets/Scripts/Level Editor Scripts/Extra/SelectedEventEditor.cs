using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SelectedEventEditor : MonoBehaviour
{
    public MoveEventHolder m_selectedMovementEvent;
    public RotEventHolder m_selectedRotationEvent;
    public FadeEventHolder m_selectedFadeEvent;

    public void SelectMoveEvent(MoveEventHolder _holder)
    {
        m_selectedMovementEvent = _holder;
        m_selectedRotationEvent = null;
        m_selectedFadeEvent = null;
    }

    public void SelectRotEvent(RotEventHolder _holder)
    {
        m_selectedMovementEvent = null;
        m_selectedRotationEvent = _holder;
        m_selectedFadeEvent = null;
    }

    public void SelectFadeEvent(FadeEventHolder _holder)
    {
        m_selectedMovementEvent = null;
        m_selectedRotationEvent = null;
        m_selectedFadeEvent = _holder;
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

            // Return corresponding lane back to previous position
            if (movIndex > 0)
            {
                correspondingLane.transform.position = chart.m_lane[lane].m_laneEventsMovement[movIndex - 1].m_targetPosition;
                correspondingLane.GetComponent<LaneHandler>().m_laneEventMovement = null;
            }
            else if(movIndex == 0)
            {
                correspondingLane.transform.position = chart.m_lane[lane].m_initialPosition;
                correspondingLane.GetComponent<LaneHandler>().m_laneEventMovement = null;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.m_currentMovementIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.m_movements[lane].m_moves.Remove(laneEvent); 
            EventListDisplay.Instance.m_movements[lane].m_objects.Remove(item);
            chart.m_lane[lane].m_laneEventsMovement.Remove(laneEvent);
            Destroy(item);
        }
        else if(m_selectedRotationEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.m_chartData;
            LaneEventRotation laneEvent = m_selectedRotationEvent.m_heldLaneEvent;
            int rotIndex = m_selectedRotationEvent.m_indexOfThis;
            int lane = m_selectedRotationEvent.m_laneID;
            GameObject item = EventListDisplay.Instance.m_rotations[lane].m_objects[rotIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.m_lanes[lane];

            // Return corresponding lane back to previous position
            if (rotIndex == chart.m_lane[lane].m_laneEventsRotation.Count)
            {
                correspondingLane.transform.rotation = Quaternion.Euler(0.0f, 0.0f, chart.m_lane[lane].m_laneEventsRotation[rotIndex - 1].m_targetRotation);
                correspondingLane.GetComponent<LaneHandler>().m_laneEventRotation = null;
            }
            else if (rotIndex == 1)
            {
                correspondingLane.transform.rotation = Quaternion.Euler(0.0f, 0.0f, chart.m_lane[lane].m_initialRotation);
                correspondingLane.GetComponent<LaneHandler>().m_laneEventRotation = null;
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
            if (fadeIndex == chart.m_lane[lane].m_laneEventFade.Count)
            {
                correspondingLane.GetComponent<SpriteShapeRenderer>().color = new Color(correspondingLane.GetComponent<SpriteShapeRenderer>().color.r, correspondingLane.GetComponent<SpriteShapeRenderer>().color.g, correspondingLane.GetComponent<SpriteShapeRenderer>().color.b, chart.m_lane[lane].m_laneEventFade[fadeIndex - 1].m_targetAlpha);
                correspondingLane.GetComponent<LaneHandler>().m_laneEventFade = null;
            }
            else if (fadeIndex == 1)
            {
                correspondingLane.GetComponent<SpriteShapeRenderer>().color = new Color(correspondingLane.GetComponent<SpriteShapeRenderer>().color.r, correspondingLane.GetComponent<SpriteShapeRenderer>().color.g, correspondingLane.GetComponent<SpriteShapeRenderer>().color.b, chart.m_lane[lane].m_initialAlpha);
                correspondingLane.GetComponent<LaneHandler>().m_laneEventFade = null;
            }

            // It never goes below 0 anyways so no harm in lowering it outside an if statement
            LevelEditorManager.Instance.m_currentFadeIndex[lane]--;

            // Remove this from all the lists
            EventListDisplay.Instance.m_fades[lane].m_fades.Remove(laneEvent);
            EventListDisplay.Instance.m_fades[lane].m_objects.Remove(item);
            chart.m_lane[lane].m_laneEventFade.Remove(laneEvent);
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
    }
}
