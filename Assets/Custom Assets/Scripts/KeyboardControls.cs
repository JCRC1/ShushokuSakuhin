using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private ObjectPooler m_pooler;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_hitSource = gameObject.AddComponent<AudioSource>();
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

                // If this is odd, move on
                if (lane.m_identifier % 2 != 0)
                {
                    continue;
                }

                if (lane.m_singleNotes.Count <= 0)
                {
                    continue;
                }

                GameObject ripple = m_pooler.GetPooledNote("Ripple");

                switch (lane.m_singleNotes.Peek().m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        m_hitSource.Play();

                        ripple.SetActive(true);
                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                        ripple.GetComponent<ParticleSystem>().loop = false;

                        ScoreController.Instance.AddPerfectHit();
                        lane.m_singleNotes.Peek().gameObject.SetActive(false);
                        lane.m_singleNotes.Dequeue();
                        break;
                    case NoteHandler.NoteState.GOOD:
                        m_hitSource.Play();

                        ripple.SetActive(true);
                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.blue;
                        ripple.GetComponent<ParticleSystem>().loop = false;

                        ScoreController.Instance.AddGoodHit();
                        lane.m_singleNotes.Peek().gameObject.SetActive(false);
                        lane.m_singleNotes.Dequeue();
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

                // If this is even, move on
                if (lane.m_identifier % 2 == 0)
                {
                    continue;
                }

                if (lane.m_singleNotes.Count <= 0)
                {
                    continue;
                }

                GameObject ripple = m_pooler.GetPooledNote("Ripple");

                switch (lane.m_singleNotes.Peek().m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        m_hitSource.Play();

                        ripple.SetActive(true);
                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;
                        ripple.GetComponent<ParticleSystem>().loop = false;

                        ScoreController.Instance.AddPerfectHit();
                        lane.m_singleNotes.Peek().gameObject.SetActive(false);
                        lane.m_singleNotes.Dequeue();
                        break;
                    case NoteHandler.NoteState.GOOD:
                        m_hitSource.Play();

                        ripple.SetActive(true);
                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red;
                        ripple.GetComponent<ParticleSystem>().loop = false;

                        ScoreController.Instance.AddGoodHit();
                        lane.m_singleNotes.Peek().gameObject.SetActive(false);
                        lane.m_singleNotes.Dequeue();
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

                // If this is odd, move on
                if (lane.m_identifier % 2 != 0)
                {
                    continue;
                }

                if (lane.m_holdNotes.Count <= 0)
                {
                    continue;
                }

                GameObject ripple = m_pooler.GetPooledNote("Ripple");

                switch (lane.m_holdNotes.Peek().m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        m_hitSource.Play();

                        ripple.SetActive(true);
                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;

                        ripple.GetComponent<ParticleSystem>().loop = true;
                        ripple.GetComponent<ParticleLifetime>().m_lifeTime = lane.m_holdNotes.Peek().m_noteData.m_duration * GameManager.Instance.m_secPerBeat;

                        lane.m_holdNotes.Peek().m_isHeld = true;
                        break;
                    case NoteHandler.NoteState.GOOD:
                        m_hitSource.Play();

                        ripple.SetActive(true);
                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.blue;

                        ripple.GetComponent<ParticleSystem>().loop = true;
                        ripple.GetComponent<ParticleLifetime>().m_lifeTime = lane.m_holdNotes.Peek().m_noteData.m_duration * GameManager.Instance.m_secPerBeat;

                        lane.m_holdNotes.Peek().m_isHeld = true;
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

                // If this is even, move on
                if (lane.m_identifier % 2 == 0)
                {
                    continue;
                }

                if (lane.m_holdNotes.Count <= 0)
                {
                    continue;
                }

                GameObject ripple = m_pooler.GetPooledNote("Ripple");

                switch (lane.m_holdNotes.Peek().m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        m_hitSource.Play();

                        ripple.SetActive(true);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red + Color.blue;

                        ripple.GetComponent<ParticleSystem>().loop = true;
                        ripple.transform.position = lane.transform.GetChild(1).position;
                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleLifetime>().m_lifeTime = lane.m_holdNotes.Peek().m_noteData.m_duration * GameManager.Instance.m_secPerBeat;

                        lane.m_holdNotes.Peek().m_isHeld = true;
                        break;
                    case NoteHandler.NoteState.GOOD:
                        m_hitSource.Play();

                        ripple.SetActive(true);
                        ripple.GetComponent<ParticleSystem>().startColor = Color.red;

                        ripple.GetComponent<ParticleSystem>().loop = true;
                        ripple.transform.position = lane.transform.GetChild(1).position;
                        ripple.GetComponent<ParticleLifetime>().m_follow = lane.transform.GetChild(1);
                        ripple.GetComponent<ParticleLifetime>().m_lifeTime = lane.m_holdNotes.Peek().m_noteData.m_duration * GameManager.Instance.m_secPerBeat;

                        lane.m_holdNotes.Peek().m_isHeld = true;
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
