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
    public KeyCode[] m_oddLaneKeybind;

    /// <summary>
    /// The Blue Lanes
    /// </summary>
    public KeyCode[] m_evenLaneKeybind;

    public AudioClip m_hitSound;
    [HideInInspector]
    public AudioSource m_hitSource;
    public AudioMixerGroup m_sfxMixerGroup;

    private ObjectPooler m_pooler;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_hitSource = gameObject.AddComponent<AudioSource>();
        m_hitSource.outputAudioMixerGroup = m_sfxMixerGroup;
        m_hitSource.volume = 0.05f;
        m_hitSource.playOnAwake = false;
        m_hitSource.clip = m_hitSound;

        m_pooler = GetComponent<ObjectPooler>();
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
        for (int i = 0; i < m_evenLaneKeybind.Length; i++)
        {
            if (Input.GetKeyDown(m_evenLaneKeybind[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool EvenKeyUp()
    {
        for (int i = 0; i < m_evenLaneKeybind.Length; i++)
        {
            if (Input.GetKeyUp(m_evenLaneKeybind[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool OddKeyDown()
    {
        for (int i = 0; i < m_oddLaneKeybind.Length; i++)
        {
            if (Input.GetKeyDown(m_oddLaneKeybind[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool OddKeyUp()
    {
        for (int i = 0; i < m_oddLaneKeybind.Length; i++)
        {
            if (Input.GetKeyUp(m_oddLaneKeybind[i]))
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
            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_identifier % 2 == 0)
                {
                    lane.GetComponent<LineRenderer>().material.SetColor("_Color", Color.blue);
                }
            }
        }

        if (OddKeyUp())
        {
            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_identifier % 2 != 0)
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
            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_identifier % 2 == 0)
                {
                    lane.GetComponent<LineRenderer>().material.SetColor("_Color", Color.blue * 4);
                }
            }
        }


        if (OddKeyDown())
        {
            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_identifier % 2 != 0)
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
            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                // Check if there is even notes enqueued
                if (lane.m_singleNotes.Count <= 0)
                {
                    continue;
                }

                if (!lane.m_allNotes.Peek().GetComponent<SingleNoteHandler>())
                {
                    continue;
                }

                SingleNoteHandler currentSingleNote = lane.m_allNotes.Peek().GetComponent<SingleNoteHandler>();

                // If this is odd, move on
                if (lane.m_identifier % 2 != 0)
                {
                    continue;
                }

                GameObject ripple = m_pooler.GetPooledNote("Ripple");
                
                GameObject perfectText = m_pooler.GetPooledNote("PerfectText");
                GameObject goodText = m_pooler.GetPooledNote("GoodText");

                switch (currentSingleNote.m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        m_hitSource.Play();
                        ScoreController.Instance.AddPerfectHit();

                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                        ripple.GetComponent<ParticleSystem>().loop = false;
                        ripple.SetActive(true);

                        perfectText.transform.position = lane.transform.GetChild(1).position;
                        perfectText.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        perfectText.SetActive(true);

                        currentSingleNote.gameObject.SetActive(false);
                        lane.m_singleNotes.Dequeue();
                        lane.m_allNotes.Dequeue();
                        break;
                    case NoteHandler.NoteState.GOOD:
                        m_hitSource.Play();
                        ScoreController.Instance.AddGoodHit();

                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.blue;
                        ripple.GetComponent<ParticleSystem>().loop = false;
                        ripple.SetActive(true);

                        goodText.transform.position = lane.transform.GetChild(1).position;
                        goodText.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        goodText.SetActive(true);

                        currentSingleNote.gameObject.SetActive(false);
                        lane.m_singleNotes.Dequeue();
                        lane.m_allNotes.Dequeue();
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
            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                // Check if there are even notes in this lane
                if (lane.m_singleNotes.Count <= 0)
                {
                    continue;
                }

                if (!lane.m_allNotes.Peek().GetComponent<SingleNoteHandler>())
                {
                    continue;
                }

                SingleNoteHandler currentSingleNote = lane.m_allNotes.Peek().GetComponent<SingleNoteHandler>();


                // If this is even, move on
                if (lane.m_identifier % 2 == 0)
                {
                    continue;
                }

                GameObject ripple = m_pooler.GetPooledNote("Ripple");

                GameObject perfectText = m_pooler.GetPooledNote("PerfectText");
                GameObject goodText = m_pooler.GetPooledNote("GoodText");

                switch (currentSingleNote.m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        m_hitSource.Play();
                        ScoreController.Instance.AddPerfectHit();

                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                        ripple.GetComponent<ParticleSystem>().loop = false;
                        ripple.SetActive(true);

                        perfectText.transform.position = lane.transform.GetChild(1).position;
                        perfectText.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        perfectText.SetActive(true);

                        currentSingleNote.gameObject.SetActive(false);
                        lane.m_singleNotes.Dequeue();
                        lane.m_allNotes.Dequeue();
                        break;
                    case NoteHandler.NoteState.GOOD:
                        m_hitSource.Play();
                        ScoreController.Instance.AddPerfectHit();

                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red;
                        ripple.GetComponent<ParticleSystem>().loop = false;
                        ripple.SetActive(true);

                        goodText.transform.position = lane.transform.GetChild(1).position;
                        goodText.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        goodText.SetActive(true);

                        currentSingleNote.gameObject.SetActive(false);
                        lane.m_singleNotes.Dequeue();
                        lane.m_allNotes.Dequeue();
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
            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_holdNotes.Count <= 0)
                {
                    continue;
                }

                if (!lane.m_allNotes.Peek().GetComponent<HoldNoteHandler>())
                {
                    continue;
                }

                HoldNoteHandler currentHoldNote = lane.m_allNotes.Peek().GetComponent<HoldNoteHandler>();

                // If this is odd, move on
                if (lane.m_identifier % 2 != 0)
                {
                    continue;
                }

                GameObject ripple = m_pooler.GetPooledNote("Ripple");

                GameObject perfectText = m_pooler.GetPooledNote("PerfectText");
                GameObject goodText = m_pooler.GetPooledNote("GoodText");

                switch (currentHoldNote.m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        KeyCode pressed = KeyCode.None;

                        if (currentHoldNote.m_pressed == KeyCode.None)
                        {
                            foreach (KeyCode key in m_evenLaneKeybind)
                            {
                                if (Input.GetKey(key))
                                {
                                    pressed = key;
                                    currentHoldNote.m_pressed = pressed;
                                }
                            }
                        }
                        currentHoldNote.m_isHeld = true;

                        if (currentHoldNote.m_pressed == pressed)
                        {
                            m_hitSource.Play();

                            ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                            ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                            ripple.GetComponent<ParticleSystem>().loop = true;
                            ripple.GetComponent<ParticleLifetime>().m_lifeTime = lane.m_holdNotes.Peek().m_noteData.m_duration * GameManager.Instance.m_secPerBeat;

                            ripple.SetActive(true);

                            perfectText.transform.position = lane.transform.GetChild(1).position;
                            perfectText.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                            perfectText.SetActive(true);
                        }                        
                        break;
                    case NoteHandler.NoteState.GOOD:
                        pressed = KeyCode.None;

                        if (currentHoldNote.m_pressed == KeyCode.None)
                        {
                            foreach (KeyCode key in m_evenLaneKeybind)
                            {
                                if (Input.GetKey(key))
                                {
                                    pressed = key;
                                    currentHoldNote.m_pressed = pressed;
                                }
                            }
                        }
                        currentHoldNote.m_isHeld = true;

                        if (currentHoldNote.m_pressed == pressed)
                        {
                            m_hitSource.Play();

                            ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                            ripple.GetComponent<ParticleSystem>().startColor = Color.blue;
                            ripple.GetComponent<ParticleSystem>().loop = true;
                            ripple.GetComponent<ParticleLifetime>().m_lifeTime = lane.m_holdNotes.Peek().m_noteData.m_duration * GameManager.Instance.m_secPerBeat;

                            ripple.SetActive(true);

                            goodText.transform.position = lane.transform.GetChild(1).position;
                            goodText.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
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
            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_holdNotes.Count <= 0)
                {
                    continue;
                }

                if (!lane.m_allNotes.Peek().GetComponent<HoldNoteHandler>())
                {
                    continue;
                }

                HoldNoteHandler currentHoldNote = lane.m_allNotes.Peek().GetComponent<HoldNoteHandler>();

                // If this is odd, move on
                if (lane.m_identifier % 2 == 0)
                {
                    continue;
                }

                GameObject ripple = m_pooler.GetPooledNote("Ripple");

                GameObject perfectText = m_pooler.GetPooledNote("PerfectText");
                GameObject goodText = m_pooler.GetPooledNote("GoodText");

                switch (currentHoldNote.m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        KeyCode pressed = KeyCode.None;

                        if (currentHoldNote.m_pressed == KeyCode.None)
                        {
                            foreach (KeyCode key in m_oddLaneKeybind)
                            {
                                if (Input.GetKey(key))
                                {
                                    pressed = key;
                                    currentHoldNote.m_pressed = pressed;
                                }
                            }
                        }
                        currentHoldNote.m_isHeld = true;

                        if (currentHoldNote.m_pressed == pressed)
                        {
                            m_hitSource.Play();

                            ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                            ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                            ripple.GetComponent<ParticleSystem>().loop = true;
                            ripple.GetComponent<ParticleLifetime>().m_lifeTime = lane.m_holdNotes.Peek().m_noteData.m_duration * GameManager.Instance.m_secPerBeat;

                            ripple.SetActive(true);

                            perfectText.transform.position = lane.transform.GetChild(1).position;
                            perfectText.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                            perfectText.SetActive(true);
                        }
                        break;
                    case NoteHandler.NoteState.GOOD:
                        pressed = KeyCode.None;

                        if (currentHoldNote.m_pressed == KeyCode.None)
                        {
                            foreach (KeyCode key in m_oddLaneKeybind)
                            {
                                if (Input.GetKey(key))
                                {
                                    pressed = key;
                                    currentHoldNote.m_pressed = pressed;
                                }
                            }
                        }
                        currentHoldNote.m_isHeld = true;

                        if (currentHoldNote.m_pressed == pressed)
                        {
                            m_hitSource.Play();

                            ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                            ripple.GetComponent<ParticleSystem>().startColor = Color.red;
                            ripple.GetComponent<ParticleSystem>().loop = true;
                            ripple.GetComponent<ParticleLifetime>().m_lifeTime = lane.m_holdNotes.Peek().m_noteData.m_duration * GameManager.Instance.m_secPerBeat;

                            ripple.SetActive(true);

                            goodText.transform.position = lane.transform.GetChild(1).position;
                            goodText.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
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
