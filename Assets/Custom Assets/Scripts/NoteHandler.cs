using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHandler : MonoBehaviour
{
    public Transform m_start;
    public Transform m_end;

    public enum NoteState
    {
        NONE,
        PERFECT,
        GOOD,
        MISS
    }

    public NoteState m_noteState;

    public virtual void Initialize(Transform _start, Transform _end)
    {
        m_noteState = NoteState.NONE;
        m_start = _start;
        m_end = _end;

        transform.position = _start.position;
        transform.rotation = _start.rotation;
    }
}
