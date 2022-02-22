using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNoteHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int m_laneID;
    public int m_indexOfThis;
    public SingleNoteData m_heldNote;

    private void Update()
    {
        m_indexOfThis = NoteListDisplay.Instance.m_singleNotes[m_laneID].m_single.IndexOf(m_heldNote);
    }
}
