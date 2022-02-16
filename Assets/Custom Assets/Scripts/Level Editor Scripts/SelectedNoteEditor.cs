using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedNoteEditor : MonoBehaviour
{
    public SingleNoteHolder m_selectedSingleNote;
    public HoldNoteHolder m_selectedHoldNote;

    public void SelectSingleNote(SingleNoteHolder _holder)
    {
        m_selectedSingleNote = _holder;
        m_selectedHoldNote = null;
    }

    public void SelectHoldNote(HoldNoteHolder _holder)
    {
        m_selectedSingleNote = null;
        m_selectedHoldNote = _holder;
    }

    public void RemoveFromList()
    {
        if (m_selectedSingleNote)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.m_chartData;
            SingleNoteData singleNote = m_selectedSingleNote.m_heldNote;
            int noteIndex = m_selectedSingleNote.m_indexOfThis;
            int lane = m_selectedSingleNote.m_laneID;
            GameObject item = NoteListDisplay.Instance.m_singleNotes[lane].m_objects[noteIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.m_lanes[lane];

            // Lower the index by one
            correspondingLane.GetComponent<LaneHandler>().m_nextSingleNoteIndex--;

            // Remove this from all the lists
            NoteListDisplay.Instance.m_singleNotes[lane].m_single.Remove(singleNote);
            NoteListDisplay.Instance.m_singleNotes[lane].m_objects.Remove(item);
            chart.m_lane[lane].m_singleNote.Remove(singleNote);
            Destroy(item);

            // Remove from Lane's note queue
            List<SingleNoteHandler> noteList = new List<SingleNoteHandler>(LevelEditorManager.Instance.m_lanes[noteIndex].GetComponent<LaneHandler>().m_singleNotes);
            noteList[noteIndex].gameObject.SetActive(false);
            noteList.RemoveAt(noteIndex);
            LevelEditorManager.Instance.m_lanes[noteIndex].GetComponent<LaneHandler>().m_singleNotes = new Queue<SingleNoteHandler>(noteList);
        }
        else if (m_selectedHoldNote)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.m_chartData;
            HoldNoteData holdNote = m_selectedHoldNote.m_heldNote;
            int noteIndex = m_selectedHoldNote.m_indexOfThis;
            int lane = m_selectedHoldNote.m_laneID;
            GameObject item = NoteListDisplay.Instance.m_holdNotes[lane].m_objects[noteIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.m_lanes[lane];

            // Lower the index by one
            correspondingLane.GetComponent<LaneHandler>().m_nextHoldNoteIndex--;

            // Remove this from all the lists
            NoteListDisplay.Instance.m_holdNotes[lane].m_hold.Remove(holdNote);
            NoteListDisplay.Instance.m_holdNotes[lane].m_objects.Remove(item);
            chart.m_lane[lane].m_holdNote.Remove(holdNote);
            Destroy(item);

            // Remove from Lane's note queue
            List<HoldNoteHandler> noteList = new List<HoldNoteHandler>(LevelEditorManager.Instance.m_lanes[noteIndex].GetComponent<LaneHandler>().m_holdNotes);
            noteList[noteIndex].gameObject.SetActive(false);
            noteList.RemoveAt(noteIndex);
            LevelEditorManager.Instance.m_lanes[noteIndex].GetComponent<LaneHandler>().m_holdNotes = new Queue<HoldNoteHandler>(noteList);
        }
    }

    public void EditSingleNote(GameObject _display)
    {
        if (m_selectedSingleNote)
        {
            _display.SetActive(true);
        }
    }

    public void EditHoldNote(GameObject _display)
    {
        if (m_selectedHoldNote)
        {
            _display.SetActive(true);
        }
    }

    public void SeekToSelectedNote()
    {
        if (m_selectedSingleNote)
        {
            LevelEditorManager.Instance.m_audioSource.time = (LevelEditorManager.Instance.m_secPerBeat * (m_selectedSingleNote.m_heldNote.m_beat - 4)) + LevelEditorManager.Instance.m_chartData.m_trackOffset;
        }
        else if (m_selectedHoldNote)
        {
            LevelEditorManager.Instance.m_audioSource.time = (LevelEditorManager.Instance.m_secPerBeat * (m_selectedHoldNote.m_heldNote.m_beat - 4)) + LevelEditorManager.Instance.m_chartData.m_trackOffset;
        }
    }
}
