using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
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
    private float m_currentRotation;

    // Where notes spawn and end
    private Transform m_startPoint;
    private Transform m_endPoint;
    private SpriteShapeController m_spriteShapeController;

    // Note spawn related
    // Holds a reference in a queue to all the notes of this lane
    public Queue<NoteHandler> m_notes;
    private int m_nextNoteIndex;

    private void Start()
    {
        m_startPoint = transform.GetChild(0);
        m_endPoint = transform.GetChild(1);

        m_spriteShapeController = GetComponent<SpriteShapeController>();

        m_laneEventMovement = new LaneEventMovement();

        m_notes = new Queue<NoteHandler>();
        m_nextNoteIndex = 0;
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

    private void Update()
    {
        LaneLengthUpdate();
        LaneMovementUpdate();
        LaneRotationUpdate();

        NoteSpawn();
        NoteJudgement();        
    }

    // To freely be able to move the start and end points and it morphs the lane with it
    void LaneLengthUpdate()
    {
        Spline spline = m_spriteShapeController.spline;
        spline.Clear();

        // Lock Y Movement
        transform.GetChild(0).localPosition = new Vector2(transform.GetChild(0).localPosition.x, 0);
        transform.GetChild(1).localPosition = new Vector2(transform.GetChild(1).localPosition.x, 0);

        GameObject start = transform.GetChild(0).gameObject;
        GameObject end = transform.GetChild(1).gameObject;

        spline.InsertPointAt(0, new Vector2(start.transform.localPosition.x + 0.5f, start.transform.localPosition.y + 0.5f));
        spline.InsertPointAt(0, new Vector2(start.transform.localPosition.x + 0.5f, start.transform.localPosition.y - 0.5f));

        spline.InsertPointAt(0, new Vector2(end.transform.localPosition.x - 0.5f, end.transform.localPosition.y - 0.5f));
        spline.InsertPointAt(0, new Vector2(end.transform.localPosition.x - 0.5f, end.transform.localPosition.y + 0.5f));

        m_spriteShapeController.RefreshSpriteShape();
    }

    private void LaneMovementUpdate()
    {
        if (GameManager.Instance)
        {
            float trackPosInBeats;
            trackPosInBeats = GameManager.Instance.m_trackPosInBeats;

            float t = (0.0f - (m_laneEventMovement.m_beat - trackPosInBeats) / m_laneEventMovement.m_duration);
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
                float t = (0.0f - (m_laneEventMovement.m_beat - trackPosInBeats) / m_laneEventMovement.m_duration);
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

            float t = (0.0f - (m_laneEventRotation.m_beat - trackPosInBeats) / m_laneEventRotation.m_duration);
            t = Mathf.Clamp01(t);

            switch (m_laneEventRotation.m_easeType)
            {
                case LaneEvent.EaseType.EASE_NONE:
                    break;

                case LaneEvent.EaseType.EASE_NORMAL:
                    Mathf.Lerp(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, (0.0f - (m_laneEventRotation.m_beat - trackPosInBeats) / m_laneEventRotation.m_duration))));
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

            m_currentRotation = Mathf.Lerp(m_startRotation, m_laneEventRotation.m_targetRotation, t);

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_currentRotation);
        }
        else
        {
            float trackPosInBeats;
            trackPosInBeats = LevelEditorManager.Instance.m_trackPosInBeats;

            float t = (0.0f - (m_laneEventRotation.m_beat - trackPosInBeats) / m_laneEventRotation.m_duration);
            t = Mathf.Clamp01(t);

            switch (m_laneEventRotation.m_easeType)
            {
                case LaneEvent.EaseType.EASE_NONE:
                    break;

                case LaneEvent.EaseType.EASE_NORMAL:
                    Mathf.Lerp(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, (0.0f - (m_laneEventRotation.m_beat - trackPosInBeats) / m_laneEventRotation.m_duration))));
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

            m_currentRotation = Mathf.Lerp(m_startRotation, m_laneEventRotation.m_targetRotation, t);

            //transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_currentRotation);
            m_laneEventRotation.m_pivotPoint = new Vector2(transform.position.x - 5.0f, transform.position.y);
            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, m_currentRotation);
            transform.position = rot * (transform.position - (Vector3)m_laneEventRotation.m_pivotPoint) + (Vector3)m_laneEventRotation.m_pivotPoint;
            transform.rotation = rot; 
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
                if (GameManager.Instance.m_chartData.m_lane[m_identifier].m_notes[m_nextNoteIndex].m_beat < GameManager.Instance.m_trackPosInBeats + GameManager.Instance.m_beatsToShow + 10)
                {
                    var note = Instantiate(GameManager.Instance.m_notePrefab, transform.GetChild(2));
                    note.GetComponent<NoteHandler>().Initialize(GameManager.Instance.m_chartData.m_lane[m_identifier].m_notes[m_nextNoteIndex], transform.GetChild(0), transform.GetChild(1));

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
                    if (LevelEditorManager.Instance.m_chartData.m_lane[m_identifier].m_notes[m_nextNoteIndex].m_beat < LevelEditorManager.Instance.m_trackPosInBeats + LevelEditorManager.Instance.m_beatsToShow + 10)
                    {
                        var note = Instantiate(LevelEditorManager.Instance.m_notePrefab, transform.GetChild(2));
                        note.GetComponent<NoteHandler>().Initialize(LevelEditorManager.Instance.m_chartData.m_lane[m_identifier].m_notes[m_nextNoteIndex], transform.GetChild(0), transform.GetChild(1));

                        m_notes.Enqueue(note.GetComponent<NoteHandler>());
                        m_nextNoteIndex++;
                    }
                }
            }
        }
    }

    public void NoteJudgement()
    {
        if (m_notes.Count > 0)
        {
            if (Vector2.Distance(m_notes.Peek().transform.position, m_endPoint.position) <= 0.5f)
            {
                Destroy(m_notes.Dequeue().gameObject);
            }
        }
    } 
}
