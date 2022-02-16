using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldNoteEditor : MonoBehaviour
{
    public HoldNoteData m_selectedNote;

    public InputField m_noteLane;
    public InputField m_noteBeat;
    public InputField m_noteDuration;

    public void SetNote(SelectedNoteEditor _holder)
    {
        if (_holder.m_selectedHoldNote != null)
        {
            m_selectedNote = _holder.m_selectedHoldNote.m_heldNote;

            m_noteLane.text = _holder.m_selectedHoldNote.m_laneID.ToString();
            m_noteBeat.text = _holder.m_selectedHoldNote.m_heldNote.m_beat.ToString();
            m_noteDuration.text = _holder.m_selectedHoldNote.m_heldNote.m_duration.ToString();
        }
    }

    public void ConfirmEdit()
    {
        m_selectedNote.m_beat = float.Parse(m_noteBeat.text);
        m_selectedNote.m_duration = float.Parse(m_noteDuration.text);
    }
}
