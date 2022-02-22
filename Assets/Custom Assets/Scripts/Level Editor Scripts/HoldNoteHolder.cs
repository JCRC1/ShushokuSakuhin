using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNoteHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int m_laneID;
    public int m_indexOfThis;
    public HoldNoteData m_heldNote;

    private void Update()
    {
        m_indexOfThis = NoteListDisplay.Instance.m_holdNotes[m_laneID].m_hold.IndexOf(m_heldNote);
    }
}
