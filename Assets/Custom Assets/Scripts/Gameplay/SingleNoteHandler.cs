using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNoteHandler : NoteHandler
{
    public SingleNoteData noteData;

    public void InitializeSingleNote(SingleNoteData _noteData, Transform _start, Transform _end)
    {
        noteData = _noteData;
        base.Initialize(_start, _end);
    }

    private void Update()
    {
        float t = 0;
        // Note Movement
        if (GameManager.Instance)
        {
            t = (1.0f - (noteData.beat - GameManager.Instance.trackPosInBeats) / GameManager.Instance.beatsToShow);

        }
        else if (LevelEditorManager.Instance)
        {
            t = (1.0f - (noteData.beat - LevelEditorManager.Instance.trackPosInBeats) / LevelEditorManager.Instance.beatsToShow);
        }

        t = Mathf.Clamp01(t);
        transform.position = Vector2.Lerp(start.position, end.position, t);

        if (LevelEditorManager.Instance)
        {
            if (noteData.beat < LevelEditorManager.Instance.trackPosInBeats)
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
