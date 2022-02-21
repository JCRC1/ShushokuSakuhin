using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleNoteEditor : MonoBehaviour
{
    public SingleNoteData m_selectedNote;

    public InputField m_noteLane;
    public InputField m_noteBeat;

    public SelectedNoteEditor m_holder;

    public void SetNote(SelectedNoteEditor _holder)
    {
        if (_holder.m_selectedSingleNote != null)
        {
            m_holder = _holder;

            m_selectedNote = _holder.m_selectedSingleNote.m_heldNote;

            m_noteLane.text = _holder.m_selectedSingleNote.m_laneID.ToString();
            m_noteBeat.text = _holder.m_selectedSingleNote.m_heldNote.m_beat.ToString();
        }
    }

    public void ConfirmEdit()
    {
        m_selectedNote.m_beat = float.Parse(m_noteBeat.text);

        NoteListDisplay.Instance.EditNote(m_selectedNote, m_holder.m_selectedSingleNote);
    }
}
