using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LaneHandler : MonoBehaviour
{
    public int identifier;

    // Lane Event related
    [HideInInspector]
    public LaneEventMovement laneEventMovement;
    [HideInInspector]
    public int movementIndex;
    [HideInInspector]
    public Vector2 movementStartPosition;

    [HideInInspector]
    public LaneEventRotation laneEventRotation;
    [HideInInspector]
    public int rotationIndex;
    [HideInInspector]
    public float startRotation;
    [HideInInspector]
    private float currentAngle;

    [HideInInspector]
    public LaneEventFade laneEventFade;
    public int fadeIndex;
    [HideInInspector]
    public float startAlpha = 1;
    [HideInInspector]
    private float currentAlpha;

    [HideInInspector]
    public LaneEventLength laneEventLength;
    [HideInInspector]
    public int lengthIndex;
    [HideInInspector]
    public float startLength = 10.0f;
    [HideInInspector]
    private float currentLength;

    // Where notes spawn and end
    private Transform startPoint;
    private Transform endPoint;
    private LineRenderer lineRenderer;

    // Note spawn related
    // Holds a reference in a queue to all the notes of this lane
    public Queue<SingleNoteHandler> singleNotes;
    public Queue<HoldNoteHandler> holdNotes;
    public Queue<GameObject> allNotes;
    public int nextSingleNoteIndex;
    public int nextHoldNoteIndex;

    private void Start()
    {
        startPoint = transform.GetChild(0);
        endPoint = transform.GetChild(1);

        //spriteShapeController = GetComponent<SpriteShapeController>();
        lineRenderer = GetComponent<LineRenderer>();

        singleNotes = new Queue<SingleNoteHandler>();
        holdNotes = new Queue<HoldNoteHandler>();
        allNotes = new Queue<GameObject>();

        nextSingleNoteIndex = 0;
        nextHoldNoteIndex = 0;
        currentAlpha = startAlpha;
        currentLength = startLength;


        // Red is Odd, Blue is Even
        if (identifier % 2 == 0)
        {
            GetComponent<LineRenderer>().startColor = Color.blue;
            GetComponent<LineRenderer>().endColor = Color.blue;
        }
        else
        {
            GetComponent<LineRenderer>().startColor = Color.red;
            GetComponent<LineRenderer>().endColor = Color.red;
        }

        // Set the rendering order of the sprites
        GetComponent<LineRenderer>().sortingOrder = 10 + identifier;
    }

    // Assign lane event to this object
    public void InitializeMovement(LaneEventMovement _laneEvent, Vector2 _startPosition)
    {
        laneEventMovement = _laneEvent;
        movementStartPosition = _startPosition;
    }

    // Assign lane event to this object
    public void InitializeRotation(LaneEventRotation _laneEvent, float _startRotation)
    {
        laneEventRotation = _laneEvent;
        startRotation = _startRotation;
    }

    // Assign lane event to this object
    public void InitializeFade(LaneEventFade _laneEvent, float _startAlpha)
    {
        laneEventFade = _laneEvent;
        startAlpha = _startAlpha;
    }

    // Assign lane event to this object
    public void InitializeLength(LaneEventLength _laneEvent, float _startLength)
    {
        laneEventLength = _laneEvent;
        startLength = _startLength;
    }

    private void Update()
    {
        SpriteShapeUpdate();

        LaneMovementUpdate();
        LaneRotationUpdate();
        LaneAlphaUpdate();
        LaneLengthUpdate();

        SingleNoteSpawn();
        HoldNoteSpawn();

        SingleNoteJudgement();
        HoldNoteJudgement();
    }

    private void SpriteShapeUpdate()
    {
        if(startPoint.localPosition.x >= 0)
        {
            lineRenderer.SetPosition(0, new Vector3(0.5f, 0.0f, 0.0f));
            lineRenderer.SetPosition(1, startPoint.localPosition + new Vector3(0.5f, 0.0f, 0.0f));
        }
        else
        {
            lineRenderer.SetPosition(0, new Vector3(-0.5f, 0.0f, 0.0f));
            lineRenderer.SetPosition(1, startPoint.localPosition - new Vector3(0.5f, 0.0f, 0.0f));
        }
    }

    // To freely be able to move the start and end points and it morphs the lane with it
    void LaneLengthUpdate()
    {
        if (GameManager.Instance)
        {
            float trackPosInBeats;
            trackPosInBeats = GameManager.Instance.trackPosInBeats;

            float t = (0.0f - (laneEventLength.beat - trackPosInBeats) / (laneEventLength.duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (laneEventLength.easeType)
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

            currentLength = Mathf.Lerp(startLength, laneEventLength.targetLength, t);

            startPoint.localPosition = new Vector2(currentLength, 0);
        }
        else
        {
            float trackPosInBeats;
            trackPosInBeats = LevelEditorManager.Instance.trackPosInBeats;

            float t = (0.0f - (laneEventLength.beat - trackPosInBeats) / (laneEventLength.duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (laneEventLength.easeType)
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

            currentLength = Mathf.Lerp(startLength, laneEventLength.targetLength, t);

            startPoint.localPosition = new Vector2(currentLength, 0);
        }
    }

    private void LaneMovementUpdate()
    {
        if (GameManager.Instance)
        {
            float trackPosInBeats;
            trackPosInBeats = GameManager.Instance.trackPosInBeats;

            float t = (0.0f - (laneEventMovement.beat - trackPosInBeats) / (laneEventMovement.duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (laneEventMovement.easeType)
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

            transform.position = Vector2.Lerp(movementStartPosition, laneEventMovement.targetPosition, t);
        }
        else
        {
            if (LevelEditorManager.Instance.initialized)
            {
                float trackPosInBeats;

                trackPosInBeats = LevelEditorManager.Instance.trackPosInBeats;
                float t = (0.0f - (laneEventMovement.beat - trackPosInBeats) / (laneEventMovement.duration + 0.001f));
                t = Mathf.Clamp01(t);

                switch (laneEventMovement.easeType)
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

                transform.position = Vector2.Lerp(movementStartPosition, laneEventMovement.targetPosition, t);
            }
        }
    }

    private void LaneRotationUpdate()
    {
        if (GameManager.Instance)
        {
            float trackPosInBeats;
            trackPosInBeats = GameManager.Instance.trackPosInBeats;

            float t = (0.0f - (laneEventRotation.beat - trackPosInBeats) / (laneEventRotation.duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (laneEventRotation.easeType)
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

            currentAngle = Mathf.Lerp(startRotation, laneEventRotation.targetRotation, t);

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, currentAngle);
        }
        else
        {
            float trackPosInBeats;
            trackPosInBeats = LevelEditorManager.Instance.trackPosInBeats;

            float t = (0.0f - (laneEventRotation.beat - trackPosInBeats) / (laneEventRotation.duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (laneEventRotation.easeType)
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
            currentAngle = Mathf.Lerp(startRotation, laneEventRotation.targetRotation, t);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, currentAngle);
        }
    }

    private void LaneAlphaUpdate()
    {
        if (GameManager.Instance)
        {
            float trackPosInBeats;
            trackPosInBeats = GameManager.Instance.trackPosInBeats;

            float t = (0.0f - (laneEventFade.beat - trackPosInBeats) / (laneEventFade.duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (laneEventFade.easeType)
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

            currentAlpha = Mathf.Lerp(startAlpha, laneEventFade.targetAlpha, t);

            GetComponent<LineRenderer>().startColor = new Color(GetComponent<LineRenderer>().startColor.r, GetComponent<LineRenderer>().startColor.g, GetComponent<LineRenderer>().startColor.b, currentAlpha);
            GetComponent<LineRenderer>().endColor = new Color(GetComponent<LineRenderer>().endColor.r, GetComponent<LineRenderer>().endColor.g, GetComponent<LineRenderer>().endColor.b, currentAlpha);

            if (laneEventFade.fadeNotes)
            {
                foreach (Transform notes in transform.GetChild(2))
                {
                    if (!notes.gameObject.activeSelf)
                    {
                        continue;
                    }
                    // Fade the single notes
                    notes.gameObject.GetComponent<SpriteRenderer>().color = new Color(notes.gameObject.GetComponent<SpriteRenderer>().color.r, notes.gameObject.GetComponent<SpriteRenderer>().color.g, notes.gameObject.GetComponent<SpriteRenderer>().color.b, currentAlpha);
                    if (notes.gameObject.GetComponent<LineRenderer>())
                    {
                        Color nLineRendererCol = notes.gameObject.GetComponent<LineRenderer>().startColor;

                        notes.gameObject.GetComponent<LineRenderer>().startColor = new Color(nLineRendererCol.r, nLineRendererCol.g, nLineRendererCol.b, currentAlpha);
                        notes.gameObject.GetComponent<LineRenderer>().endColor = new Color(nLineRendererCol.r, nLineRendererCol.g, nLineRendererCol.b, currentAlpha);
                    }
                }
            }

            transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(transform.GetChild(1).GetComponent<SpriteRenderer>().color.r, transform.GetChild(1).GetComponent<SpriteRenderer>().color.g, transform.GetChild(1).GetComponent<SpriteRenderer>().color.b, currentAlpha);

        }
        else
        {
            float trackPosInBeats;
            trackPosInBeats = LevelEditorManager.Instance.trackPosInBeats;

            float t = (0.0f - (laneEventFade.beat - trackPosInBeats) / (laneEventFade.duration + 0.001f));
            t = Mathf.Clamp01(t);

            switch (laneEventFade.easeType)
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

            currentAlpha = Mathf.Lerp(startAlpha, laneEventFade.targetAlpha, t);

            GetComponent<LineRenderer>().startColor = new Color(GetComponent<LineRenderer>().startColor.r, GetComponent<LineRenderer>().startColor.g, GetComponent<LineRenderer>().startColor.b, currentAlpha);
            GetComponent<LineRenderer>().endColor = new Color(GetComponent<LineRenderer>().endColor.r, GetComponent<LineRenderer>().endColor.g, GetComponent<LineRenderer>().endColor.b, currentAlpha);

            if (laneEventFade.fadeNotes)
            {
                foreach (Transform notes in transform.GetChild(2))
                {
                    if (!notes.gameObject.activeSelf)
                    {
                        continue;
                    }
                    // Fade the single notes
                    notes.gameObject.GetComponent<SpriteRenderer>().color = new Color(notes.gameObject.GetComponent<SpriteRenderer>().color.r, notes.gameObject.GetComponent<SpriteRenderer>().color.g, notes.gameObject.GetComponent<SpriteRenderer>().color.b, currentAlpha);
                    
                    if (notes.gameObject.GetComponent<LineRenderer>())
                    {
                        Color nLineRendererCol = notes.gameObject.GetComponent<LineRenderer>().startColor;

                        notes.gameObject.GetComponent<LineRenderer>().startColor = new Color(nLineRendererCol.r, nLineRendererCol.g, nLineRendererCol.b, currentAlpha);
                        notes.gameObject.GetComponent<LineRenderer>().endColor = new Color(nLineRendererCol.r, nLineRendererCol.g, nLineRendererCol.b, currentAlpha);
                    }
                }
            }

            transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(transform.GetChild(1).GetComponent<SpriteRenderer>().color.r, transform.GetChild(1).GetComponent<SpriteRenderer>().color.g, transform.GetChild(1).GetComponent<SpriteRenderer>().color.b, currentAlpha);

        }
    }

    public void SingleNoteSpawn()
    {
        if (GameManager.Instance)
        {
            // First check if the current index is less than the total count of notes
            if (nextSingleNoteIndex < GameManager.Instance.chartData.lane[identifier].singleNote.Count)
            {
                // Check if the track position in beats has surpassed the beat of the notes to spawn, and if so, spawn them
                if (GameManager.Instance.chartData.lane[identifier].singleNote[nextSingleNoteIndex].beat < GameManager.Instance.trackPosInBeats + GameManager.Instance.beatsToShow)
                {
                    SingleNoteHandler note = (transform.GetChild(2).GetComponent<ObjectPooler>().GetPooledNote("Single Note")).GetComponent<SingleNoteHandler>();
                    note.InitializeSingleNote(GameManager.Instance.chartData.lane[identifier].singleNote[nextSingleNoteIndex], transform.GetChild(0), transform.GetChild(1));

                    if (note != null)
                    {
                        note.gameObject.SetActive(true);
                    }

                    singleNotes.Enqueue(note.GetComponent<SingleNoteHandler>());
                    allNotes.Enqueue(note.gameObject);
                    nextSingleNoteIndex++;
                }
            }
        }
        else
        {
            // First check if the current index is less than the total count of notes
            if (LevelEditorManager.Instance.initialized)
            {
                if (nextSingleNoteIndex < LevelEditorManager.Instance.chartData.lane[identifier].singleNote.Count)
                {
                    // Check if the track position in beats has surpassed the beat of the notes to spawn, and if so, spawn them
                    if (LevelEditorManager.Instance.chartData.lane[identifier].singleNote[nextSingleNoteIndex].beat < LevelEditorManager.Instance.trackPosInBeats + LevelEditorManager.Instance.beatsToShow)
                    {
                        SingleNoteHandler note = (transform.GetChild(2).GetComponent<ObjectPooler>().GetPooledNote("Single Note")).GetComponent<SingleNoteHandler>();
                        note.InitializeSingleNote(LevelEditorManager.Instance.chartData.lane[identifier].singleNote[nextSingleNoteIndex], transform.GetChild(0), transform.GetChild(1));

                        if (note != null)
                        {
                            note.gameObject.SetActive(true);
                        }

                        singleNotes.Enqueue(note.GetComponent<SingleNoteHandler>());
                        nextSingleNoteIndex++;
                    }
                }
            }
        }
    }

    public void HoldNoteSpawn()
    {
        if (GameManager.Instance)
        {
            // First check if the current index is less than the total count of notes
            if (nextHoldNoteIndex < GameManager.Instance.chartData.lane[identifier].holdNote.Count)
            {
                // Check if the track position in beats has surpassed the beat of the notes to spawn, and if so, spawn them
                if (GameManager.Instance.chartData.lane[identifier].holdNote[nextHoldNoteIndex].beat < GameManager.Instance.trackPosInBeats + GameManager.Instance.beatsToShow)
                {
                    HoldNoteHandler note = (transform.GetChild(2).GetComponent<ObjectPooler>().GetPooledNote("Hold Note")).GetComponent<HoldNoteHandler>();
                    note.InitializeHoldNote(GameManager.Instance.chartData.lane[identifier].holdNote[nextHoldNoteIndex], transform.GetChild(0), transform.GetChild(1), identifier);

                    if (note != null)
                    {
                        note.gameObject.SetActive(true);
                    }

                    holdNotes.Enqueue(note.GetComponent<HoldNoteHandler>()); 
                    allNotes.Enqueue(note.gameObject);
                    nextHoldNoteIndex++;
                }
            }
        }
        else
        {
            // First check if the current index is less than the total count of notes
            if (LevelEditorManager.Instance.initialized)
            {
                if (nextHoldNoteIndex < LevelEditorManager.Instance.chartData.lane[identifier].holdNote.Count)
                {
                    // Check if the track position in beats has surpassed the beat of the notes to spawn, and if so, spawn them
                    if (LevelEditorManager.Instance.chartData.lane[identifier].holdNote[nextHoldNoteIndex].beat < LevelEditorManager.Instance.trackPosInBeats + LevelEditorManager.Instance.beatsToShow)
                    {
                        HoldNoteHandler note = (transform.GetChild(2).GetComponent<ObjectPooler>().GetPooledNote("Hold Note")).GetComponent<HoldNoteHandler>();
                        note.InitializeHoldNote(LevelEditorManager.Instance.chartData.lane[identifier].holdNote[nextHoldNoteIndex], transform.GetChild(0), transform.GetChild(1), identifier);

                        if (note != null)
                        {
                            note.gameObject.SetActive(true);
                        }

                        holdNotes.Enqueue(note.GetComponent<HoldNoteHandler>());
                        nextHoldNoteIndex++;
                    }
                }
            }
        }
    }

    public void SingleNoteJudgement()
    {
        if (singleNotes.Count <= 0)
        {
            return;
        }

        if (GameManager.Instance)
        {
            if (!allNotes.Peek().GetComponent<SingleNoteHandler>())
            {
                return;
            }

            SingleNoteHandler note = allNotes.Peek().GetComponent<SingleNoteHandler>();

            if (note.noteData.beat - GameManager.Instance.trackPosInBeats > 2 * GameManager.Instance.hitWindow &&
                note.noteData.beat - GameManager.Instance.trackPosInBeats < 4 * GameManager.Instance.hitWindow)
            {
                note.noteState = NoteHandler.NoteState.GOOD;
            }
            else if (note.noteData.beat - GameManager.Instance.trackPosInBeats > 0                               &&
                     note.noteData.beat - GameManager.Instance.trackPosInBeats < 2 * GameManager.Instance.hitWindow)
            {
                note.noteState = NoteHandler.NoteState.PERFECT;
            }
            else if (note.noteData.beat - GameManager.Instance.trackPosInBeats < -GameManager.Instance.hitWindow * 4)
            {
                GameObject missText = GameManager.Instance.GetComponent<ObjectPooler>().GetPooledNote("MissText");
                missText.transform.position = endPoint.position;
                missText.GetComponent<ParticleLifetime>().follow = endPoint;
                missText.SetActive(true);

                ScoreController.Instance.currentCombo = 0;
                ScoreController.Instance.missCount++;
                note.noteState = NoteHandler.NoteState.MISS;
                note.gameObject.SetActive(false);
                singleNotes.Dequeue();
                allNotes.Dequeue();
            }
        }
    }


    public void HoldNoteJudgement()
    {
        if (holdNotes.Count <= 0)
        {
            return;
        }

        if (GameManager.Instance)
        {
            if (!allNotes.Peek().GetComponent<HoldNoteHandler>())
            {
                return;
            }

            HoldNoteHandler note = allNotes.Peek().GetComponent<HoldNoteHandler>();

            if (note.isHeld)
            {
                return;
            }

            if (note.noteData.beat - GameManager.Instance.trackPosInBeats > 2 * GameManager.Instance.hitWindow &&
                note.noteData.beat - GameManager.Instance.trackPosInBeats < 4 * GameManager.Instance.hitWindow)
            {
                note.noteState = NoteHandler.NoteState.GOOD;
            }
            else if (note.noteData.beat - GameManager.Instance.trackPosInBeats > 0 &&
                     note.noteData.beat - GameManager.Instance.trackPosInBeats < 2 * GameManager.Instance.hitWindow)
            {
                note.noteState = NoteHandler.NoteState.PERFECT;
            }
            else if (note.noteData.beat - GameManager.Instance.trackPosInBeats < -GameManager.Instance.hitWindow * 4)
            {
                GameObject missText = GameManager.Instance.GetComponent<ObjectPooler>().GetPooledNote("MissText");
                missText.transform.position = endPoint.position;
                missText.GetComponent<ParticleLifetime>().follow = endPoint;
                missText.SetActive(true);

                ScoreController.Instance.currentCombo = 0;
                ScoreController.Instance.missCount++;
                note.noteState = NoteHandler.NoteState.MISS;

                note.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                note.GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, 0.5f);
                note.GetComponent<LineRenderer>().endColor = new Color(1, 1, 1, 0.5f);

                holdNotes.Dequeue();
                allNotes.Dequeue();
            }
        }
    }
}
