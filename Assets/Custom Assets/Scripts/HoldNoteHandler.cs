using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNoteHandler : NoteHandler
{
    public HoldNoteData m_noteData;
    public bool m_isHeld;

    public int m_laneItBelongs;

    public KeyCode m_pressed;

    public void InitializeHoldNote(HoldNoteData _noteData, Transform _start, Transform _end, int _laneItBelongs)
    {
        m_pressed = KeyCode.None;
        m_isHeld = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, 1f);
        GetComponent<LineRenderer>().endColor = new Color(1, 1, 1, 1f);
        m_noteState = NoteState.NONE;
        m_noteData = _noteData;
        m_laneItBelongs = _laneItBelongs;
        base.Initialize(_start, _end);
    }

    private void Update()
    {
        float t1 = 0;
        float t2 = 0;
        // Note Movement
        if (GameManager.Instance)
        {
            t1 = (1.0f - (m_noteData.m_beat - GameManager.Instance.m_trackPosInBeats) / GameManager.Instance.m_beatsToShow);
            t2 = (1.0f - ((m_noteData.m_beat + m_noteData.m_duration) - GameManager.Instance.m_trackPosInBeats) / GameManager.Instance.m_beatsToShow);

        }
        else if (LevelEditorManager.Instance)
        {
            t1 = (1.0f - (m_noteData.m_beat - LevelEditorManager.Instance.m_trackPosInBeats) / LevelEditorManager.Instance.m_beatsToShow);
            t2 = (1.0f - ((m_noteData.m_beat + m_noteData.m_duration) - LevelEditorManager.Instance.m_trackPosInBeats) / LevelEditorManager.Instance.m_beatsToShow);
        }

        t1 = Mathf.Clamp01(t1);
        t2 = Mathf.Clamp01(t2);

        transform.position = Vector2.Lerp(m_start.position, m_end.position, t1);
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, Vector2.Lerp(m_start.position, m_end.position, t2));

        if (GameManager.Instance)
        {
            // If we are holding this note down
            if (m_isHeld)
            {
                // Check if it has reached the end and calculate score
                if (m_noteData.m_beat + m_noteData.m_duration < GameManager.Instance.m_trackPosInBeats)
                {
                    switch (m_noteState)
                    {
                        case NoteState.NONE:
                            break;
                        case NoteState.PERFECT:
                            ScoreController.Instance.AddPerfectHit();
                            KeyboardControls.Instance.m_hitSource.Play();
                            transform.parent.parent.GetComponent<LaneHandler>().m_holdNotes.Dequeue();
                            break;
                        case NoteState.GOOD:
                            ScoreController.Instance.AddGoodHit();
                            KeyboardControls.Instance.m_hitSource.Play();
                            transform.parent.parent.GetComponent<LaneHandler>().m_holdNotes.Dequeue();
                            break;
                        case NoteState.MISS:
                            break;
                    }
                    gameObject.SetActive(false);
                }
                // If while holding, we let our key up and we are not done then this note becomes miss
                if (m_laneItBelongs % 2 == 0)
                {
                    foreach (KeyCode key in KeyboardControls.Instance.m_evenLaneKeybind)
                    {
                        if (Input.GetKey(key))
                        {
                            m_pressed = key;
                        }
                    }

                    if (Input.GetKeyUp(m_pressed))
                    {
                        if (m_noteData.m_beat + m_noteData.m_duration > GameManager.Instance.m_trackPosInBeats)
                        {
                            foreach (GameObject gO in GameManager.Instance.GetComponent<ObjectPooler>().pooledNotes)
                            {
                                if (gO.activeSelf)
                                {
                                    if (gO.GetComponent<ParticleLifetime>().m_follow == transform.parent.parent.GetChild(1))
                                    {
                                        gO.SetActive(false);
                                    }
                                }
                            }

                            m_noteState = NoteState.MISS;
                            m_isHeld = false;
                        }
                    }
                } 
                else if(m_laneItBelongs % 2 != 0)
                {
                    foreach (KeyCode key in KeyboardControls.Instance.m_oddLaneKeybind)
                    {
                        if (Input.GetKey(key))
                        {
                            m_pressed = key;
                        }
                    }

                    if (Input.GetKeyUp(m_pressed))
                    {
                        if (m_noteData.m_beat + m_noteData.m_duration > GameManager.Instance.m_trackPosInBeats)
                        {
                            foreach (GameObject gO in GameManager.Instance.GetComponent<ObjectPooler>().pooledNotes)
                            {
                                if (gO.activeSelf)
                                {
                                    if (gO.GetComponent<ParticleLifetime>().m_follow == transform.parent.parent.GetChild(1))
                                    {
                                        gO.SetActive(false);
                                    }
                                }
                            }
                            m_noteState = NoteState.MISS;
                            m_isHeld = false;
                        }
                    }
                }
            }
            else
            {
                // Note continues as normal until disappearing
                if (m_noteData.m_beat + m_noteData.m_duration < GameManager.Instance.m_trackPosInBeats)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else if (LevelEditorManager.Instance)
        {
            if (m_noteData.m_beat + m_noteData.m_duration < LevelEditorManager.Instance.m_trackPosInBeats)
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
