using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHandler : MonoBehaviour
{
    public SingleNoteData m_noteData;
    private Transform m_start;
    private Transform m_end;

    public enum NoteState
    { 
        NONE,
        PERFECT,
        GOOD,
        MISS
    }

    public NoteState m_noteState;

    public void Initialize(SingleNoteData _noteData, Transform _start, Transform _end)
    {
        m_noteState = NoteState.NONE;
        m_noteData = _noteData;
        m_start = _start;
        m_end = _end;
    }

    private void Update()
    {
        float t = 0;
        // Note Movement
        if (GameManager.Instance)
        {
            t = (1.0f - (m_noteData.m_beat - GameManager.Instance.m_trackPosInBeats) / GameManager.Instance.m_beatsToShow);

        }
        else if (LevelEditorManager.Instance)
        {
            t = (1.0f - (m_noteData.m_beat - LevelEditorManager.Instance.m_trackPosInBeats) / LevelEditorManager.Instance.m_beatsToShow);
        }

        t = Mathf.Clamp01(t);
        transform.position = Vector2.Lerp(m_start.position, m_end.position, t);

        if (LevelEditorManager.Instance)
        {
            NoteHandler temp = new NoteHandler();

            if (m_noteData.m_beat < LevelEditorManager.Instance.m_trackPosInBeats)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
