using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHandler : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public enum NoteState
    {
        NONE,
        PERFECT,
        GOOD,
        MISS
    }

    public NoteState noteState;

    public virtual void Initialize(Transform _start, Transform _end)
    {
        noteState = NoteState.NONE;
        start = _start;
        end = _end;

        transform.position = _start.position;
        transform.rotation = _start.rotation;
    }
}
