using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LaneHandler : MonoBehaviour
{
    public int m_identifier;

    // Lane Event related
    public LaneEventMovement m_laneEventMovement;
    public int m_movementIndex;
    public Vector2 m_movementStartPosition;

    public LaneEventRotation m_laneEventRotation;
    public int m_rotationIndex;
    public float m_startRotation;
    private float m_currentAngle;

    public LaneEventFade m_laneEventFade;
    public int m_fadeIndex;
    public float m_startAlpha = 1;
    private float m_currentAlpha;

    public LaneEventLength m_laneEventLength;
    public int m_lengthIndex;
    public float m_startLength = 10.0f;
    private float m_currentLength;

    // Where notes spawn and end
    private Transform m_startPoint;
    private Transform m_endPoint;
    private LineRenderer m_lineRenderer;

    // Note spawn related
    // Holds a reference in a queue to all the notes of this lane
    public Queue<NoteHandler> m_notes;
    private int m_nextNoteIndex;

    private void Start()
    {
        m_startPoint = transform.GetChild(0);
        m_endPoint = transform.GetChild(1);

        //m_spriteShapeController = GetComponent<SpriteShapeController>();
        m_lineRenderer = GetComponent<LineRenderer>();

        m_notes = new Queue<NoteHandler>();
        m_nextNoteIndex = 0;
        m_currentAlpha = m_startAlpha;
        m_currentLength = m_startLength;

        if (m_identifier % 2 == 0)
        {
            GetComponent<LineRenderer>().startColor = Color.red;
            GetComponent<LineRenderer>().endColor = Color.red;
        }
        else
        {
            GetComponent<LineRenderer>().startColor = Color.blue;
            GetComponent<LineRenderer>().endColor = Color.blue;
        }
    }

    // Assign lane event to this object
    public void InitializeMovement(LaneEventMovement _laneEvent, Vector2 _startPosition)
    {
        m_laneEventMovement = _laneEvent;
        m_movementStartPosition = _startPosition;
    }

    // Assign lane event to this object
    public void InitializeRotation(LaneEventRotation _laneEvent, float _startRotation)
    {
        m_laneEventRotation = _laneEvent;
        m_startRotation = _startRotation;
    }

    // Assign lane event to this object
    public void InitializeFade(LaneEventFade _laneEvent, float _startAlpha)
    {
        m_laneEventFade = _laneEvent;
        m_startAlpha = _startAlpha;
    }

    // Assign lane event to this object
    public void InitializeLength(LaneEventLength _laneEvent, float _startLength)
    {
        m_laneEventLength = _laneEvent;
        m_startLength = _startLength;
    }

    private void Update()
    {
        SpriteShapeUpdate();

        LaneMovementUpdate();
        LaneRotationUpdate();
        LaneAlphaUpdate();
        LaneLengthUpdate();

        NoteSpawn();
        NoteJudgement();
    }

    private void SpriteShapeUpdate()
    {
        if(m_startPoint.localPosition.x >= 0)
        {
            m_lineRenderer.SetPosition(0, new Vector3(0.5f, 0.0f, 0.0f));
            m_lineRenderer.SetPosition(1, m_startPoint.localPosition + new Vector3(0.5f, 0.0f, 0.0f));
        }
        else
        {
            m_lineRenderer.SetPosition(0, new Vector3(-0.5f, 0.0f, 0.0f));
            m_lineRenderer.SetPosition(1, m_startPoint.localPosition - new Vector3(0.5f, 0.0f, 0.0f));
        }
    }

    // To freely be able to move the start and end points and it morphs the lane with it
    void LaneLengthUpdate()
    {
        if (GameManager.Instance)
        {
            float trackPosInBeats;
            trackPosInBeats = GameManager.Instance.m_trackPosInBeats;

            float t = (0.0f - (m_laneEventLength.m_beat - trackPosInBeats) / (m_laneEventLength.m_duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (m_laneEventLength.m_easeType)
            {
                case LaneEvent.EaseType.EASE_NONE:
                    break;

                case LaneEvent.EaseType.EASE_NORMAL:
                    t = Mathf.Lerp(0.0f, 1.0f, t);
                    break;

                case LaneEvent.EaseType.EASE_IN:
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;

                case LaneEvent.EaseType.EASE_OUT:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;

                default:
                    break;
            }

            m_currentLength = Mathf.Lerp(m_startLength, m_laneEventLength.m_targetLength, t);

            m_startPoint.localPosition = new Vector2(m_currentLength, 0);
        }
        else
        {
            float trackPosInBeats;
            trackPosInBeats = LevelEditorManager.Instance.m_trackPosInBeats;

            float t = (0.0f - (m_laneEventLength.m_beat - trackPosInBeats) / (m_laneEventLength.m_duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (m_laneEventLength.m_easeType)
            {
                case LaneEvent.EaseType.EASE_NONE:
                    break;

                case LaneEvent.EaseType.EASE_NORMAL:
                    t = Mathf.Lerp(0.0f, 1.0f, t);
                    break;

                case LaneEvent.EaseType.EASE_IN:
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;

                case LaneEvent.EaseType.EASE_OUT:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;

                default:
                    break;
            }

            m_currentLength = Mathf.Lerp(m_startLength, m_laneEventLength.m_targetLength, t);

            m_startPoint.localPosition = new Vector2(m_currentLength, 0);
        }
    }

    private void LaneMovementUpdate()
    {
        if (GameManager.Instance)
        {
            float trackPosInBeats;
            trackPosInBeats = GameManager.Instance.m_trackPosInBeats;

            float t = (0.0f - (m_laneEventMovement.m_beat - trackPosInBeats) / (m_laneEventMovement.m_duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (m_laneEventMovement.m_easeType)
            {
                case LaneEvent.EaseType.EASE_NONE:
                    break;

                case LaneEvent.EaseType.EASE_NORMAL:
                    t = Mathf.SmoothStep(0.0f, 1.0f, t);
                    break;

                case LaneEvent.EaseType.EASE_IN:
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;

                case LaneEvent.EaseType.EASE_OUT:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;

                default:
                    break;
            }

            transform.position = Vector2.Lerp(m_movementStartPosition, m_laneEventMovement.m_targetPosition, t);
        }
        else
        {
            if (LevelEditorManager.Instance.m_initialized)
            {
                float trackPosInBeats;

                trackPosInBeats = LevelEditorManager.Instance.m_trackPosInBeats;
                float t = (0.0f - (m_laneEventMovement.m_beat - trackPosInBeats) / (m_laneEventMovement.m_duration + 0.001f));
                t = Mathf.Clamp01(t);

                switch (m_laneEventMovement.m_easeType)
                {
                    case LaneEvent.EaseType.EASE_NONE:
                        break;

                    case LaneEvent.EaseType.EASE_NORMAL:
                        t = Mathf.SmoothStep(0.0f, 1.0f, t);
                        break;

                    case LaneEvent.EaseType.EASE_IN:
                        t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                        break;

                    case LaneEvent.EaseType.EASE_OUT:
                        t = Mathf.Sin(t * Mathf.PI * 0.5f);
                        break;

                    default:
                        break;
                }

                transform.position = Vector2.Lerp(m_movementStartPosition, m_laneEventMovement.m_targetPosition, t);
            }
        }
    }

    private void LaneRotationUpdate()
    {
        if (GameManager.Instance)
        {
            float trackPosInBeats;
            trackPosInBeats = GameManager.Instance.m_trackPosInBeats;

            float t = (0.0f - (m_laneEventRotation.m_beat - trackPosInBeats) / (m_laneEventRotation.m_duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (m_laneEventRotation.m_easeType)
            {
                case LaneEvent.EaseType.EASE_NONE:
                    break;

                case LaneEvent.EaseType.EASE_NORMAL:
                    t = Mathf.Lerp(0.0f, 1.0f, t);
                    break;

                case LaneEvent.EaseType.EASE_IN:
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;

                case LaneEvent.EaseType.EASE_OUT:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;

                default:
                    break;
            }

            m_currentAngle = Mathf.Lerp(m_startRotation, m_laneEventRotation.m_targetRotation, t);

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_currentAngle);
        }
        else
        {
            float trackPosInBeats;
            trackPosInBeats = LevelEditorManager.Instance.m_trackPosInBeats;

            float t = (0.0f - (m_laneEventRotation.m_beat - trackPosInBeats) / (m_laneEventRotation.m_duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (m_laneEventRotation.m_easeType)
            {
                case LaneEvent.EaseType.EASE_NONE:
                    break;

                case LaneEvent.EaseType.EASE_NORMAL:
                    t = Mathf.Lerp(0.0f, 1.0f, t);
                    break;

                case LaneEvent.EaseType.EASE_IN:
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;

                case LaneEvent.EaseType.EASE_OUT:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;

                default:
                    break;
            }
            m_currentAngle = Mathf.Lerp(m_startRotation, m_laneEventRotation.m_targetRotation, t);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_currentAngle);
        }
    }

    private void LaneAlphaUpdate()
    {
        if (GameManager.Instance)
        {
            float trackPosInBeats;
            trackPosInBeats = GameManager.Instance.m_trackPosInBeats;

            float t = (0.0f - (m_laneEventFade.m_beat - trackPosInBeats) / (m_laneEventFade.m_duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (m_laneEventFade.m_easeType)
            {
                case LaneEvent.EaseType.EASE_NONE:
                    break;

                case LaneEvent.EaseType.EASE_NORMAL:
                    t = Mathf.Lerp(0.0f, 1.0f, t);
                    break;

                case LaneEvent.EaseType.EASE_IN:
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;

                case LaneEvent.EaseType.EASE_OUT:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;

                default:
                    break;
            }

            m_currentAlpha = Mathf.Lerp(m_startAlpha, m_laneEventFade.m_targetAlpha, t);

            GetComponent<LineRenderer>().startColor = new Color(GetComponent<LineRenderer>().startColor.r, GetComponent<LineRenderer>().startColor.g, GetComponent<LineRenderer>().startColor.b, m_currentAlpha);
            GetComponent<LineRenderer>().endColor = new Color(GetComponent<LineRenderer>().endColor.r, GetComponent<LineRenderer>().endColor.g, GetComponent<LineRenderer>().endColor.b, m_currentAlpha);

            if (m_laneEventFade.m_fadeNotes)
            {
                foreach (Transform notes in transform.GetChild(2))
                {
                    if (!notes.gameObject.activeSelf)
                    {
                        return;
                    }
                    // Fade the single notes
                    notes.gameObject.GetComponent<SpriteRenderer>().color = new Color(notes.gameObject.GetComponent<SpriteRenderer>().color.r, notes.gameObject.GetComponent<SpriteRenderer>().color.g, notes.gameObject.GetComponent<SpriteRenderer>().color.b, m_currentAlpha);
                }
            }

            transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(transform.GetChild(1).GetComponent<SpriteRenderer>().color.r, transform.GetChild(1).GetComponent<SpriteRenderer>().color.g, transform.GetChild(1).GetComponent<SpriteRenderer>().color.b, m_currentAlpha);

        }
        else
        {
            float trackPosInBeats;
            trackPosInBeats = LevelEditorManager.Instance.m_trackPosInBeats;

            float t = (0.0f - (m_laneEventFade.m_beat - trackPosInBeats) / (m_laneEventFade.m_duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (m_laneEventFade.m_easeType)
            {
                case LaneEvent.EaseType.EASE_NONE:
                    break;

                case LaneEvent.EaseType.EASE_NORMAL:
                    t = Mathf.Lerp(0.0f, 1.0f, t);
                    break;

                case LaneEvent.EaseType.EASE_IN:
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;

                case LaneEvent.EaseType.EASE_OUT:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;

                default:
                    break;
            }

            m_currentAlpha = Mathf.Lerp(m_startAlpha, m_laneEventFade.m_targetAlpha, t);

            GetComponent<LineRenderer>().startColor = new Color(GetComponent<LineRenderer>().startColor.r, GetComponent<LineRenderer>().startColor.g, GetComponent<LineRenderer>().startColor.b, m_currentAlpha);
            GetComponent<LineRenderer>().endColor = new Color(GetComponent<LineRenderer>().endColor.r, GetComponent<LineRenderer>().endColor.g, GetComponent<LineRenderer>().endColor.b, m_currentAlpha);

            if (m_laneEventFade.m_fadeNotes)
            {
                foreach (Transform notes in transform.GetChild(2))
                {
                    if (!notes.gameObject.activeSelf)
                    {
                        return;
                    }
                    // Fade the single notes
                    notes.gameObject.GetComponent<SpriteRenderer>().color = new Color(notes.gameObject.GetComponent<SpriteRenderer>().color.r, notes.gameObject.GetComponent<SpriteRenderer>().color.g, notes.gameObject.GetComponent<SpriteRenderer>().color.b, m_currentAlpha);
                }
            }

            transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(transform.GetChild(1).GetComponent<SpriteRenderer>().color.r, transform.GetChild(1).GetComponent<SpriteRenderer>().color.g, transform.GetChild(1).GetComponent<SpriteRenderer>().color.b, m_currentAlpha);

        }
    }

    public void NoteSpawn()
    {
        if (GameManager.Instance)
        {
            // First check if the current index is less than the total count of notes
            if (m_nextNoteIndex < GameManager.Instance.m_chartData.m_lane[m_identifier].m_notes.Count)
            {
                // Check if the track position in beats has surpassed the beat of the notes to spawn, and if so, spawn them
                if (GameManager.Instance.m_chartData.m_lane[m_identifier].m_notes[m_nextNoteIndex].m_beat < GameManager.Instance.m_trackPosInBeats + GameManager.Instance.m_beatsToShow)
                {
                    NoteHandler note = (transform.GetChild(2).GetComponent<ObjectPooler>().GetPooledNote("Single Note")).GetComponent<NoteHandler>();
                    note.Initialize(GameManager.Instance.m_chartData.m_lane[m_identifier].m_notes[m_nextNoteIndex], transform.GetChild(0), transform.GetChild(1));

                    if (note != null)
                    {
                        note.gameObject.SetActive(true);
                    }

                    m_notes.Enqueue(note.GetComponent<NoteHandler>());
                    m_nextNoteIndex++;
                }
            }
        }
        else
        {
            // First check if the current index is less than the total count of notes
            if (LevelEditorManager.Instance.m_initialized)
            {
                if (m_nextNoteIndex < LevelEditorManager.Instance.m_chartData.m_lane[m_identifier].m_notes.Count)
                {
                    // Check if the track position in beats has surpassed the beat of the notes to spawn, and if so, spawn them
                    if (LevelEditorManager.Instance.m_chartData.m_lane[m_identifier].m_notes[m_nextNoteIndex].m_beat < LevelEditorManager.Instance.m_trackPosInBeats + LevelEditorManager.Instance.m_beatsToShow)
                    {
                        NoteHandler note = (transform.GetChild(2).GetComponent<ObjectPooler>().GetPooledNote("Single Note")).GetComponent<NoteHandler>();
                        note.Initialize(LevelEditorManager.Instance.m_chartData.m_lane[m_identifier].m_notes[m_nextNoteIndex], transform.GetChild(0), transform.GetChild(1));

                        if (note != null)
                        {
                            note.gameObject.SetActive(true);
                        }

                        m_notes.Enqueue(note.GetComponent<NoteHandler>());
                        m_nextNoteIndex++;
                    }
                }
            }
        }
    }

    public void NoteJudgement()
    {
        if (m_notes.Count <= 0)
        {
            return;
        }

        if (GameManager.Instance)
        {
            if (m_notes.Peek().m_noteData.m_beat - GameManager.Instance.m_trackPosInBeats > GameManager.Instance.m_hitWindow &&
                m_notes.Peek().m_noteData.m_beat - GameManager.Instance.m_trackPosInBeats < 10 * GameManager.Instance.m_hitWindow)
            {
                m_notes.Peek().m_noteState = NoteHandler.NoteState.GOOD;
            }
            else if (m_notes.Peek().m_noteData.m_beat - GameManager.Instance.m_trackPosInBeats > 0                               &&
                     m_notes.Peek().m_noteData.m_beat - GameManager.Instance.m_trackPosInBeats < GameManager.Instance.m_hitWindow)
            {
                m_notes.Peek().m_noteState = NoteHandler.NoteState.PERFECT;
            }
            else if (m_notes.Peek().m_noteData.m_beat - GameManager.Instance.m_trackPosInBeats < -GameManager.Instance.m_hitWindow * 5)
            {
                Debug.Log("MISS");
                ScoreController.Instance.m_currentCombo = 0;
                m_notes.Peek().m_noteState = NoteHandler.NoteState.MISS;
                m_notes.Peek().gameObject.SetActive(false);
                m_notes.Dequeue();
            }
        }
    }
}
