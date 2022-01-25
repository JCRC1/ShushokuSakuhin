using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedEventEditor : MonoBehaviour
{
    public MoveEventHolder m_selectedMovementEvent;
    public RotEventHolder m_selectedRotationEvent;

    public void SelectMoveEvent(MoveEventHolder _holder)
    {
        m_selectedMovementEvent = _holder;
        m_selectedRotationEvent = null;
    }

    public void SelectRotEvent(RotEventHolder _holder)
    {
        m_selectedMovementEvent = null;
        m_selectedRotationEvent = _holder;
    }

    public void RemoveFromList()
    {
        if (m_selectedMovementEvent && !m_selectedRotationEvent)
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
        else if(!m_selectedMovementEvent && m_selectedRotationEvent)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.m_chartData;
            LaneEventRotation laneEvent = m_selectedRotationEvent.m_heldLaneEvent;
            int rotIndex = m_selectedRotationEvent.m_indexOfThis;
            int lane = m_selectedRotationEvent.m_laneID;
            GameObject item = EventListDisplay.Instance.m_rotations[lane].m_objects[rotIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.m_lanes[lane];

            // Return corresponding lane back to previous position
            if (rotIndex == chart.m_lane[lane].m_laneEventsMovement.Count)
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
    }

    public void EditMoveEvent(GameObject _display)
    {
        if (m_selectedMovementEvent && !m_selectedRotationEvent)
        {
            _display.SetActive(true);           
        }
    }

    public void EditRotEvent(GameObject _display)
    {
        if (!m_selectedMovementEvent && m_selectedRotationEvent)
        {
            _display.SetActive(true);
        }
    }
    public void SeekToSelectedEvent()
    {
        if (m_selectedMovementEvent && !m_selectedRotationEvent)
        {
            LevelEditorManager.Instance.m_audioSource.time = (LevelEditorManager.Instance.m_secPerBeat * m_selectedMovementEvent.m_heldLaneEvent.m_beat) + LevelEditorManager.Instance.m_chartData.m_trackOffset;
        }
        else if (!m_selectedMovementEvent && m_selectedRotationEvent)
        {
            LevelEditorManager.Instance.m_audioSource.time = (LevelEditorManager.Instance.m_secPerBeat * m_selectedRotationEvent.m_heldLaneEvent.m_beat) + LevelEditorManager.Instance.m_chartData.m_trackOffset;
        }
    }
}
