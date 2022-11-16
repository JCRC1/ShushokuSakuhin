using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedNoteEditor : MonoBehaviour
{
    public SingleNoteHolder selectedSingleNote;
    public HoldNoteHolder selectedHoldNote;

    public void SelectSingleNote(SingleNoteHolder _holder)
    {
        selectedSingleNote = _holder;
        selectedHoldNote = null;
    }

    public void SelectHoldNote(HoldNoteHolder _holder)
    {
        selectedSingleNote = null;
        selectedHoldNote = _holder;
    }

    public void RemoveFromList()
    {
        if (selectedSingleNote)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.chartData;
            SingleNoteData singleNote = selectedSingleNote.heldNote;
            int noteIndex = selectedSingleNote.indexOfThis;
            int lane = selectedSingleNote.laneID;
            GameObject item = NoteListDisplay.Instance.singleNotes[lane].objects[noteIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.lanes[lane];

            // Lower the index by one
            correspondingLane.GetComponent<LaneHandler>().nextSingleNoteIndex--;

            // Remove this from all the lists
            NoteListDisplay.Instance.singleNotes[lane].single.Remove(singleNote);
            NoteListDisplay.Instance.singleNotes[lane].objects.Remove(item);
            chart.lane[lane].singleNote.Remove(singleNote);
            Destroy(item);

            // Remove from Lane's note queue
            List<SingleNoteHandler> noteList = new List<SingleNoteHandler>(LevelEditorManager.Instance.lanes[lane].GetComponent<LaneHandler>().singleNotes);
            noteList[noteIndex].gameObject.SetActive(false);
            noteList.RemoveAt(noteIndex);
            LevelEditorManager.Instance.lanes[lane].GetComponent<LaneHandler>().singleNotes = new Queue<SingleNoteHandler>(noteList);
        }
        else if (selectedHoldNote)
        {
            // References to all the members we actually need, though mostly for making things less spaghetti
            ChartData chart = LevelEditorManager.Instance.chartData;
            HoldNoteData holdNote = selectedHoldNote.heldNote;
            int noteIndex = selectedHoldNote.indexOfThis;
            int lane = selectedHoldNote.laneID;
            GameObject item = NoteListDisplay.Instance.holdNotes[lane].objects[noteIndex];
            GameObject correspondingLane = LevelEditorManager.Instance.lanes[lane];

            // Lower the index by one
            correspondingLane.GetComponent<LaneHandler>().nextHoldNoteIndex--;

            // Remove this from all the lists
            NoteListDisplay.Instance.holdNotes[lane].hold.Remove(holdNote);
            NoteListDisplay.Instance.holdNotes[lane].objects.Remove(item);
            chart.lane[lane].holdNote.Remove(holdNote);
            Destroy(item);

            // Remove from Lane's note queue
            List<HoldNoteHandler> noteList = new List<HoldNoteHandler>(LevelEditorManager.Instance.lanes[lane].GetComponent<LaneHandler>().holdNotes);
            noteList[noteIndex].gameObject.SetActive(false);
            noteList.RemoveAt(noteIndex);
            LevelEditorManager.Instance.lanes[lane].GetComponent<LaneHandler>().holdNotes = new Queue<HoldNoteHandler>(noteList);
        }
    }

    public void EditSingleNote(GameObject _display)
    {
        if (selectedSingleNote)
        {
            _display.SetActive(true);
        }
    }

    public void EditHoldNote(GameObject _display)
    {
        if (selectedHoldNote)
        {
            _display.SetActive(true);
        }
    }

    public void SeekToSelectedNote()
    {
        if (selectedSingleNote)
        {
            LevelEditorManager.Instance.audioSource.time = (LevelEditorManager.Instance.secPerBeat * (selectedSingleNote.heldNote.beat - 4)) + LevelEditorManager.Instance.chartData.trackOffset;
        }
        else if (selectedHoldNote)
        {
            LevelEditorManager.Instance.audioSource.time = (LevelEditorManager.Instance.secPerBeat * (selectedHoldNote.heldNote.beat - 4)) + LevelEditorManager.Instance.chartData.trackOffset;
        }
    }
}
