using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHandler : MonoBehaviour
{
    public NoteData m_noteData;
    private Transform m_start;
    private Transform m_end;

    public void Initialize(NoteData _noteData, Transform _start, Transform _end)
    {
        m_noteData = _noteData;
        m_start = _start;
        m_end = _end;
    }

    private void Update()
    {
        // Note Movement
        transform.position = Vector2.Lerp(m_start.position, m_end.position, GameManager.Instance.m_beatsToShow - (m_noteData.m_beat - GameManager.Instance.m_trackPosInBeats));
    }
}
