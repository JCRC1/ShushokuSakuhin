using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class KeyboardControls : MonoBehaviour
{
    public static KeyboardControls Instance;

    /// <summary>
    /// The Red Lanes
    /// </summary>
    public KeyCode[] oddLaneKeybind;

    /// <summary>
    /// The Blue Lanes
    /// </summary>
    public KeyCode[] evenLaneKeybind;

    public AudioClip hitSound;
    [HideInInspector]
    public AudioSource hitSource;
    public AudioMixerGroup sfxMixerGroup;

    private ObjectPooler pooler;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hitSource = gameObject.AddComponent<AudioSource>();
        hitSource.outputAudioMixerGroup = sfxMixerGroup;
        hitSource.volume = 0.05f;
        hitSource.playOnAwake = false;
        hitSource.clip = hitSound;

        pooler = GetComponent<ObjectPooler>();
    }

    private void Update()
    {
        SingleNoteDetection();
        HoldNoteDetection();

        LaneHighlight();
        LaneColorReturn();
    }

    public bool EvenKeyDown()
    {
        for (int i = 0; i < evenLaneKeybind.Length; i++)
        {
            if (Input.GetKeyDown(evenLaneKeybind[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool EvenKeyUp()
    {
        for (int i = 0; i < evenLaneKeybind.Length; i++)
        {
            if (Input.GetKeyUp(evenLaneKeybind[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool OddKeyDown()
    {
        for (int i = 0; i < oddLaneKeybind.Length; i++)
        {
            if (Input.GetKeyDown(oddLaneKeybind[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool OddKeyUp()
    {
        for (int i = 0; i < oddLaneKeybind.Length; i++)
        {
            if (Input.GetKeyUp(oddLaneKeybind[i]))
            {
                return true;
            }
        }
        return false;
    }

    private void LaneColorReturn()
    {
        if (EvenKeyUp())
        {
            for (int j = 0; j < GameManager.Instance.lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.lanes[j].GetComponent<LaneHandler>();

                if (lane.identifier % 2 == 0)
                {
                    lane.GetComponent<LineRenderer>().material.SetColor("_Color", Color.blue);
                }
            }
        }

        if (OddKeyUp())
        {
            for (int j = 0; j < GameManager.Instance.lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.lanes[j].GetComponent<LaneHandler>();

                if (lane.identifier % 2 != 0)
                {
                    lane.GetComponent<LineRenderer>().material.SetColor("_Color", Color.red);
                }
            }
        }
    }

    private void LaneHighlight()
    {
        if (EvenKeyDown())
        {
            for (int j = 0; j < GameManager.Instance.lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.lanes[j].GetComponent<LaneHandler>();

                if (lane.identifier % 2 == 0)
                {
                    lane.GetComponent<LineRenderer>().material.SetColor("_Color", Color.blue * 4);
                }
            }
        }


        if (OddKeyDown())
        {
            for (int j = 0; j < GameManager.Instance.lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.lanes[j].GetComponent<LaneHandler>();

                if (lane.identifier % 2 != 0)
                {
                    lane.GetComponent<LineRenderer>().material.SetColor("_Color", Color.red * 4);
                }
            }
        }
    }

    private void SingleNoteDetection()
    {
        // Even
        if (EvenKeyDown())
        {
            for (int j = 0; j < GameManager.Instance.lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.lanes[j].GetComponent<LaneHandler>();

                // Check if there is even notes enqueued
                if (lane.singleNotes.Count <= 0)
                {
                    continue;
                }

                if (!lane.allNotes.Peek().GetComponent<SingleNoteHandler>())
                {
                    continue;
                }

                SingleNoteHandler currentSingleNote = lane.allNotes.Peek().GetComponent<SingleNoteHandler>();

                // If this is odd, move on
                if (lane.identifier % 2 != 0)
                {
                    continue;
                }

                GameObject ripple = pooler.GetPooledNote("Ripple");
                GameObject rippleG = pooler.GetPooledNote("RippleGood");

                GameObject perfectText = pooler.GetPooledNote("PerfectText");
                GameObject goodText = pooler.GetPooledNote("GoodText");

                switch (currentSingleNote.noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        hitSource.Play();
                        ScoreController.Instance.AddPerfectHit();

                        ripple.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                        ripple.GetComponent<ParticleSystem>().loop = false;
                        ripple.SetActive(true);

                        perfectText.transform.position = lane.transform.GetChild(1).position;
                        perfectText.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                        perfectText.SetActive(true);

                        currentSingleNote.gameObject.SetActive(false);
                        lane.singleNotes.Dequeue();
                        lane.allNotes.Dequeue();
                        break;
                    case NoteHandler.NoteState.GOOD:
                        hitSource.Play();
                        ScoreController.Instance.AddGoodHit();

                        rippleG.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                        rippleG.GetComponent<ParticleSystem>().startColor = Color.blue;
                        rippleG.GetComponent<ParticleSystem>().loop = false;
                        rippleG.SetActive(true);

                        goodText.transform.position = lane.transform.GetChild(1).position;
                        goodText.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                        goodText.SetActive(true);

                        currentSingleNote.gameObject.SetActive(false);
                        lane.singleNotes.Dequeue();
                        lane.allNotes.Dequeue();
                        break;
                    case NoteHandler.NoteState.MISS:
                        break;
                    default:
                        break;
                }
            }
        }

        // Odd
        if (OddKeyDown())
        {
            for (int j = 0; j < GameManager.Instance.lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.lanes[j].GetComponent<LaneHandler>();

                // Check if there are even notes in this lane
                if (lane.singleNotes.Count <= 0)
                {
                    continue;
                }

                if (!lane.allNotes.Peek().GetComponent<SingleNoteHandler>())
                {
                    continue;
                }

                SingleNoteHandler currentSingleNote = lane.allNotes.Peek().GetComponent<SingleNoteHandler>();


                // If this is even, move on
                if (lane.identifier % 2 == 0)
                {
                    continue;
                }

                GameObject ripple = pooler.GetPooledNote("Ripple");
                GameObject rippleG = pooler.GetPooledNote("RippleGood");

                GameObject perfectText = pooler.GetPooledNote("PerfectText");
                GameObject goodText = pooler.GetPooledNote("GoodText");

                switch (currentSingleNote.noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        hitSource.Play();
                        ScoreController.Instance.AddPerfectHit();

                        ripple.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                        ripple.GetComponent<ParticleSystem>().loop = false;
                        ripple.SetActive(true);

                        perfectText.transform.position = lane.transform.GetChild(1).position;
                        perfectText.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                        perfectText.SetActive(true);

                        currentSingleNote.gameObject.SetActive(false);
                        lane.singleNotes.Dequeue();
                        lane.allNotes.Dequeue();
                        break;
                    case NoteHandler.NoteState.GOOD:
                        hitSource.Play();
                        ScoreController.Instance.AddPerfectHit();

                        rippleG.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                        rippleG.GetComponent<ParticleSystem>().startColor = Color.red;
                        rippleG.GetComponent<ParticleSystem>().loop = false;
                        rippleG.SetActive(true);

                        goodText.transform.position = lane.transform.GetChild(1).position;
                        goodText.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                        goodText.SetActive(true);

                        currentSingleNote.gameObject.SetActive(false);
                        lane.singleNotes.Dequeue();
                        lane.allNotes.Dequeue();
                        break;
                    case NoteHandler.NoteState.MISS:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void HoldNoteDetection()
    {
        // Even
        if (EvenKeyDown())
        {
            for (int j = 0; j < GameManager.Instance.lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.lanes[j].GetComponent<LaneHandler>();

                if (lane.holdNotes.Count <= 0)
                {
                    continue;
                }

                if (!lane.allNotes.Peek().GetComponent<HoldNoteHandler>())
                {
                    continue;
                }

                HoldNoteHandler currentHoldNote = lane.allNotes.Peek().GetComponent<HoldNoteHandler>();

                // If this is odd, move on
                if (lane.identifier % 2 != 0)
                {
                    continue;
                }

                GameObject ripple = pooler.GetPooledNote("Ripple");
                GameObject rippleG = pooler.GetPooledNote("RippleGood");

                GameObject perfectText = pooler.GetPooledNote("PerfectText");
                GameObject goodText = pooler.GetPooledNote("GoodText");

                switch (currentHoldNote.noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        KeyCode pressed = KeyCode.None;

                        if (currentHoldNote.pressed == KeyCode.None)
                        {
                            foreach (KeyCode key in evenLaneKeybind)
                            {
                                if (Input.GetKey(key))
                                {
                                    pressed = key;
                                    currentHoldNote.pressed = pressed;
                                }
                            }
                        }
                        currentHoldNote.isHeld = true;

                        if (currentHoldNote.pressed == pressed)
                        {
                            hitSource.Play();

                            ripple.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                            ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                            ripple.GetComponent<ParticleSystem>().loop = true;
                            ripple.GetComponent<ParticleLifetime>().lifeTime = lane.holdNotes.Peek().noteData.duration * GameManager.Instance.secPerBeat;

                            ripple.SetActive(true);

                            perfectText.transform.position = lane.transform.GetChild(1).position;
                            perfectText.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                            perfectText.SetActive(true);
                        }                        
                        break;
                    case NoteHandler.NoteState.GOOD:
                        pressed = KeyCode.None;

                        if (currentHoldNote.pressed == KeyCode.None)
                        {
                            foreach (KeyCode key in evenLaneKeybind)
                            {
                                if (Input.GetKey(key))
                                {
                                    pressed = key;
                                    currentHoldNote.pressed = pressed;
                                }
                            }
                        }
                        currentHoldNote.isHeld = true;

                        if (currentHoldNote.pressed == pressed)
                        {
                            hitSource.Play();

                            rippleG.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                            rippleG.GetComponent<ParticleSystem>().startColor = Color.blue;
                            rippleG.GetComponent<ParticleSystem>().loop = true;
                            rippleG.GetComponent<ParticleLifetime>().lifeTime = lane.holdNotes.Peek().noteData.duration * GameManager.Instance.secPerBeat;

                            rippleG.SetActive(true);

                            goodText.transform.position = lane.transform.GetChild(1).position;
                            goodText.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                            goodText.SetActive(true);
                        }
                        break;
                    case NoteHandler.NoteState.MISS:
                        break;
                    default:
                        break;
                }
            }
        }

        // Odd
        if (OddKeyDown())
        {
            for (int j = 0; j < GameManager.Instance.lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.lanes[j].GetComponent<LaneHandler>();

                if (lane.holdNotes.Count <= 0)
                {
                    continue;
                }

                if (!lane.allNotes.Peek().GetComponent<HoldNoteHandler>())
                {
                    continue;
                }

                HoldNoteHandler currentHoldNote = lane.allNotes.Peek().GetComponent<HoldNoteHandler>();

                // If this is odd, move on
                if (lane.identifier % 2 == 0)
                {
                    continue;
                }

                GameObject ripple = pooler.GetPooledNote("Ripple");
                GameObject rippleG = pooler.GetPooledNote("RippleGood");

                GameObject perfectText = pooler.GetPooledNote("PerfectText");
                GameObject goodText = pooler.GetPooledNote("GoodText");

                switch (currentHoldNote.noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        KeyCode pressed = KeyCode.None;

                        if (currentHoldNote.pressed == KeyCode.None)
                        {
                            foreach (KeyCode key in oddLaneKeybind)
                            {
                                if (Input.GetKey(key))
                                {
                                    pressed = key;
                                    currentHoldNote.pressed = pressed;
                                }
                            }
                        }
                        currentHoldNote.isHeld = true;

                        if (currentHoldNote.pressed == pressed)
                        {
                            hitSource.Play();

                            ripple.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                            ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                            ripple.GetComponent<ParticleSystem>().loop = true;
                            ripple.GetComponent<ParticleLifetime>().lifeTime = lane.holdNotes.Peek().noteData.duration * GameManager.Instance.secPerBeat;

                            ripple.SetActive(true);

                            perfectText.transform.position = lane.transform.GetChild(1).position;
                            perfectText.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                            perfectText.SetActive(true);
                        }
                        break;
                    case NoteHandler.NoteState.GOOD:
                        pressed = KeyCode.None;

                        if (currentHoldNote.pressed == KeyCode.None)
                        {
                            foreach (KeyCode key in oddLaneKeybind)
                            {
                                if (Input.GetKey(key))
                                {
                                    pressed = key;
                                    currentHoldNote.pressed = pressed;
                                }
                            }
                        }
                        currentHoldNote.isHeld = true;

                        if (currentHoldNote.pressed == pressed)
                        {
                            hitSource.Play();

                            rippleG.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                            rippleG.GetComponent<ParticleSystem>().startColor = Color.red;
                            rippleG.GetComponent<ParticleSystem>().loop = true;
                            rippleG.GetComponent<ParticleLifetime>().lifeTime = lane.holdNotes.Peek().noteData.duration * GameManager.Instance.secPerBeat;

                            rippleG.SetActive(true);

                            goodText.transform.position = lane.transform.GetChild(1).position;
                            goodText.GetComponent<ParticleLifetime>().follow = lane.transform.GetChild(1);
                            goodText.SetActive(true);
                        }
                        break;
                    case NoteHandler.NoteState.MISS:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
