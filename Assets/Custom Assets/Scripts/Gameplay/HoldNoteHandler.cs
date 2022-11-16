using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNoteHandler : NoteHandler
{
    public HoldNoteData noteData;
    public bool isHeld;

    public int laneItBelongs;

    public KeyCode pressed;

    public void InitializeHoldNote(HoldNoteData _noteData, Transform _start, Transform _end, int _laneItBelongs)
    {
        pressed = KeyCode.None;
        isHeld = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, 1f);
        GetComponent<LineRenderer>().endColor = new Color(1, 1, 1, 1f);
        noteState = NoteState.NONE;
        noteData = _noteData;
        laneItBelongs = _laneItBelongs;
        base.Initialize(_start, _end);
    }

    private void Update()
    {
        float t1 = 0;
        float t2 = 0;
        // Note Movement
        if (GameManager.Instance)
        {
            t1 = (1.0f - (noteData.beat - GameManager.Instance.trackPosInBeats) / GameManager.Instance.beatsToShow);
            t2 = (1.0f - ((noteData.beat + noteData.duration) - GameManager.Instance.trackPosInBeats) / GameManager.Instance.beatsToShow);

        }
        else if (LevelEditorManager.Instance)
        {
            t1 = (1.0f - (noteData.beat - LevelEditorManager.Instance.trackPosInBeats) / LevelEditorManager.Instance.beatsToShow);
            t2 = (1.0f - ((noteData.beat + noteData.duration) - LevelEditorManager.Instance.trackPosInBeats) / LevelEditorManager.Instance.beatsToShow);
        }

        t1 = Mathf.Clamp01(t1);
        t2 = Mathf.Clamp01(t2);

        transform.position = Vector2.Lerp(start.position, end.position, t1);
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, Vector2.Lerp(start.position, end.position, t2));

        if (GameManager.Instance)
        {
            // If we are holding this note down
            if (isHeld)
            {
                // Check if it has reached the end and calculate score
                if (noteData.beat + noteData.duration < GameManager.Instance.trackPosInBeats)
                {
                    Debug.Log("Fine to let go");
                    switch (noteState)
                    {
                        case NoteState.NONE:
                            break;
                        case NoteState.PERFECT:
                            ScoreController.Instance.AddPerfectHit();
                            KeyboardControls.Instance.hitSource.Play();
                            transform.parent.parent.GetComponent<LaneHandler>().holdNotes.Dequeue();
                            transform.parent.parent.GetComponent<LaneHandler>().allNotes.Dequeue();
                            break;
                        case NoteState.GOOD:
                            ScoreController.Instance.AddGoodHit();
                            KeyboardControls.Instance.hitSource.Play();
                            transform.parent.parent.GetComponent<LaneHandler>().holdNotes.Dequeue();
                            transform.parent.parent.GetComponent<LaneHandler>().allNotes.Dequeue();
                            break;
                        case NoteState.MISS:
                            break;
                    }
                    gameObject.SetActive(false);
                }
                // If while holding, we let our key up and we are not done then this note becomes miss
                if (laneItBelongs % 2 == 0)
                {
                    foreach (KeyCode key in KeyboardControls.Instance.evenLaneKeybind)
                    {
                        if (Input.GetKey(key))
                        {
                            pressed = key;
                        }
                    }

                    if (Input.GetKeyUp(pressed))
                    {
                        if (noteData.beat + noteData.duration > GameManager.Instance.trackPosInBeats)
                        {
                            foreach (GameObject gO in GameManager.Instance.GetComponent<ObjectPooler>().pooledNotes)
                            {
                                if (gO.activeSelf)
                                {
                                    if (gO.GetComponent<ParticleLifetime>().follow == transform.parent.parent.GetChild(1))
                                    {
                                        gO.SetActive(false);
                                    }
                                }
                            }
                            noteState = NoteState.MISS;

                            GameObject missText = GameManager.Instance.GetComponent<ObjectPooler>().GetPooledNote("MissText");
                            missText.transform.position = transform.parent.parent.GetChild(1).position;
                            missText.GetComponent<ParticleLifetime>().follow = transform.parent.parent.GetChild(1);
                            missText.SetActive(true);

                            ScoreController.Instance.currentCombo = 0;
                            ScoreController.Instance.missCount++;

                            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                            GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, 0.5f);
                            GetComponent<LineRenderer>().endColor = new Color(1, 1, 1, 0.5f);

                            transform.parent.parent.GetComponent<LaneHandler>().holdNotes.Dequeue();
                            transform.parent.parent.GetComponent<LaneHandler>().allNotes.Dequeue();
                            isHeld = false;
                        }
                    }
                } 
                else if(laneItBelongs % 2 != 0)
                {
                    foreach (KeyCode key in KeyboardControls.Instance.oddLaneKeybind)
                    {
                        if (Input.GetKey(key))
                        {
                            pressed = key;
                        }
                    }

                    if (Input.GetKeyUp(pressed))
                    {
                        if (noteData.beat + noteData.duration > GameManager.Instance.trackPosInBeats)
                        {
                            foreach (GameObject gO in GameManager.Instance.GetComponent<ObjectPooler>().pooledNotes)
                            {
                                if (gO.activeSelf)
                                {
                                    if (gO.GetComponent<ParticleLifetime>().follow == transform.parent.parent.GetChild(1))
                                    {
                                        gO.SetActive(false);
                                    }
                                }
                            }
                            noteState = NoteState.MISS;

                            GameObject missText = GameManager.Instance.GetComponent<ObjectPooler>().GetPooledNote("MissText");
                            missText.transform.position = transform.parent.parent.GetChild(1).position;
                            missText.GetComponent<ParticleLifetime>().follow = transform.parent.parent.GetChild(1);
                            missText.SetActive(true);

                            ScoreController.Instance.currentCombo = 0;
                            ScoreController.Instance.missCount++;

                            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                            GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, 0.5f);
                            GetComponent<LineRenderer>().endColor = new Color(1, 1, 1, 0.5f);

                            transform.parent.parent.GetComponent<LaneHandler>().holdNotes.Dequeue();
                            transform.parent.parent.GetComponent<LaneHandler>().allNotes.Dequeue();
                            isHeld = false; 
                        }
                    }
                }
            }
            else
            {
                // Note continues as normal until disappearing
                if (noteData.beat + noteData.duration < GameManager.Instance.trackPosInBeats)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else if (LevelEditorManager.Instance)
        {
            if (noteData.beat + noteData.duration < LevelEditorManager.Instance.trackPosInBeats)
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
