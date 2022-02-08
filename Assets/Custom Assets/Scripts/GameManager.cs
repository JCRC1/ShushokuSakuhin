using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //----------------------------
    // Chart information
    //----------------------------
    /// <summary>
    /// From Resources folder
    /// </summary>
    public string m_path;
    //[HideInInspector]
    public ChartData m_chartData;

    // Prefab of note
    public GameObject m_notePrefab;
    // Prefab of lane
    public GameObject m_lanePrefab;

    [HideInInspector]
    public List<GameObject> m_lanes;

    // Indexes
    [HideInInspector]
    public List<int> m_nextNoteIndex;
    //[HideInInspector]
    public List<int> m_currentMovementIndex;
    //[HideInInspector]
    public List<int> m_currentRotationIndex;
    //[HideInInspector]
    public List<int> m_currentFadeIndex;
    //[HideInInspector]
    public List<int> m_currentLengthIndex;

    // Total lanes
    [HideInInspector]
    public int m_totalLanes;
    public int m_totalNotes;
    // Beats to show in advance
    public float m_beatsToShow = 4;
    // Seconds between each beat
    [HideInInspector]
    public float m_secPerBeat;
    public float m_hitWindow;

    // Position of track in seconds
    public float m_trackPos;
    // Position of track in beats
    [HideInInspector]
    public float m_trackPosInBeats;

    //the number of beats in each loop
    public float m_beatsPerLoop;
    //the total number of loops completed since the looping clip first started
    public int m_completedLoops = 0;
    //The current position of the song within the loop in beats.
    public float m_loopPosInBeats;
    //The current relative position of the song within the loop measured between 0 and 1.
    public float m_loopPosInAnalog;

    // Attached AudioSource
    [HideInInspector]
    public AudioSource m_audioSource;

    public bool m_initialized;
    public bool m_finalized;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (m_initialized)
        {
            TrackUpdate();
            LaneEventSpawn();
        }
    }

    private void Initialize()
    {
        m_chartData = new ChartData();

        string json = File.ReadAllText("C:/Unity Projects/ShushokuSakuhin/Assets/Custom Assets/Resources/" + m_path);
        JsonUtility.FromJsonOverwrite(json, m_chartData);

        // Initialize lanes and index array
        {
            m_totalLanes = 0;
            m_lanes = new List<GameObject>();
            m_nextNoteIndex = new List<int>();
            m_currentMovementIndex = new List<int>();
            m_currentRotationIndex = new List<int>();
            m_currentFadeIndex = new List<int>();
            m_currentLengthIndex = new List<int>();

        for (int i = 0; i < m_chartData.m_lane.Count; i++)
            {
                InitLoadedLane(m_chartData.m_lane[i]);

                m_totalNotes += m_chartData.m_lane[i].m_notes.Count;
            }
        }

        m_audioSource = GetComponent<AudioSource>();

        m_secPerBeat = 60.0f / m_chartData.m_trackBPM;

        m_audioSource.clip = Resources.Load<AudioClip>(m_chartData.m_trackAudioPath);
        m_audioSource.Play();
        m_initialized = true;
    }

    public void InitLoadedLane(LaneData _laneData)
    {
        m_lanes.Add(Instantiate(m_lanePrefab, _laneData.m_initialPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, _laneData.m_initialRotation))));

        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_identifier = m_totalLanes;

        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_movementStartPosition = _laneData.m_initialPosition;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_laneEventMovement.m_targetPosition = _laneData.m_initialPosition;

        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_startRotation = _laneData.m_initialRotation;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_laneEventRotation.m_targetRotation = _laneData.m_initialRotation;

        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_startAlpha = 1.0f;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_laneEventFade.m_targetAlpha = 1.0f;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_startLength = 10.0f;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_laneEventLength.m_targetLength = 10.0f;

        m_nextNoteIndex.Add(0);
        m_currentMovementIndex.Add(0);
        m_currentRotationIndex.Add(0);
        m_currentFadeIndex.Add(0);
        m_currentLengthIndex.Add(0);

        m_totalLanes++;
    }

    private void TrackUpdate()
    {
        m_secPerBeat = 60.0f / m_chartData.m_trackBPM;
        // How many seconds since we started
        m_trackPos = (float)(m_audioSource.time - m_chartData.m_trackOffset);
        // How many beats since we started
        m_trackPosInBeats = m_trackPos / m_secPerBeat;

        if (m_trackPosInBeats >= (m_completedLoops + 1) * m_beatsPerLoop)
            m_completedLoops++;
        m_loopPosInBeats = m_trackPosInBeats - m_completedLoops * m_beatsPerLoop;

        m_loopPosInAnalog = m_loopPosInBeats / m_beatsPerLoop;

        if (m_trackPos == m_audioSource.clip.length)
        {
            m_finalized = true;
            m_initialized = false;
        }
    }

    private void LaneEventSpawn()
    {
        for (int i = 0; i < m_chartData.m_lane.Count; i++)
        {
            // Lane Events
            {
                // Make sure these don't go negative or else Unity has a cry
                if (m_currentMovementIndex[i] <= 0)
                {
                    m_currentMovementIndex[i] = 0;
                }

                // First check if the current index is less than the total count of events
                if (m_currentMovementIndex[i] < m_chartData.m_lane[i].m_laneEventsMovement.Count)
                {
                    if (m_chartData.m_lane[i].m_laneEventsMovement[m_currentMovementIndex[i]].m_beat < m_trackPosInBeats)
                    {
                        if (m_currentMovementIndex[i] < 1)
                        {
                            m_lanes[i].GetComponent<LaneHandler>().InitializeMovement(m_chartData.m_lane[i].m_laneEventsMovement[m_currentMovementIndex[i]], m_chartData.m_lane[i].m_initialPosition);

                            m_currentMovementIndex[i]++;
                        }
                        else
                        {
                            m_lanes[i].GetComponent<LaneHandler>().InitializeMovement(m_chartData.m_lane[i].m_laneEventsMovement[m_currentMovementIndex[i]], m_chartData.m_lane[i].m_laneEventsMovement[m_currentMovementIndex[i] - 1].m_targetPosition);

                            m_currentMovementIndex[i]++;
                        }
                    }
                }

                if (m_currentRotationIndex[i] <= 0)
                {
                    m_currentRotationIndex[i] = 0;
                }

                // First check if the current index is less than the total count of events
                if (m_currentRotationIndex[i] < m_chartData.m_lane[i].m_laneEventsRotation.Count)
                {
                    if (m_chartData.m_lane[i].m_laneEventsRotation[m_currentRotationIndex[i]].m_beat < m_trackPosInBeats)
                    {
                        if (m_currentRotationIndex[i] < 1)
                        {
                            float z = m_chartData.m_lane[i].m_initialRotation;
                            m_lanes[i].GetComponent<LaneHandler>().InitializeRotation(m_chartData.m_lane[i].m_laneEventsRotation[m_currentRotationIndex[i]], z);

                            m_currentRotationIndex[i]++;
                        }
                        else
                        {
                            float z = m_chartData.m_lane[i].m_laneEventsRotation[m_currentRotationIndex[i] - 1].m_targetRotation;

                            if (z >= 360)
                            {
                                z %= 360;
                            }
                            m_lanes[i].GetComponent<LaneHandler>().InitializeRotation(m_chartData.m_lane[i].m_laneEventsRotation[m_currentRotationIndex[i]], z);

                            m_currentRotationIndex[i]++;
                        }
                    }
                }

                // Fade
                if (m_currentFadeIndex[i] <= 0)
                {
                    m_currentFadeIndex[i] = 0;
                }

                // First check if the current index is less than the total count of events
                if (m_currentFadeIndex[i] < m_chartData.m_lane[i].m_laneEventFade.Count)
                {
                    if (m_chartData.m_lane[i].m_laneEventFade[m_currentFadeIndex[i]].m_beat < m_trackPosInBeats)
                    {
                        if (m_currentFadeIndex[i] < 1)
                        {
                            float a = m_chartData.m_lane[i].m_initialAlpha;
                            m_lanes[i].GetComponent<LaneHandler>().InitializeFade(m_chartData.m_lane[i].m_laneEventFade[m_currentFadeIndex[i]], a);

                            m_currentFadeIndex[i]++;
                        }
                        else
                        {
                            float a = m_chartData.m_lane[i].m_laneEventFade[m_currentFadeIndex[i] - 1].m_targetAlpha;

                            if (a >= 1)
                                a = 1;
                            else if (a <= 0)
                                a = 0;

                            m_lanes[i].GetComponent<LaneHandler>().InitializeFade(m_chartData.m_lane[i].m_laneEventFade[m_currentFadeIndex[i]], a);

                            m_currentFadeIndex[i]++;
                        }
                    }
                }

                // Length
                if (m_currentLengthIndex[i] <= 0)
                {
                    m_currentLengthIndex[i] = 0;
                }

                // First check if the current index is less than the total count of events
                if (m_currentLengthIndex[i] < m_chartData.m_lane[i].m_laneEventLength.Count)
                {
                    if (m_chartData.m_lane[i].m_laneEventLength[m_currentLengthIndex[i]].m_beat < m_trackPosInBeats)
                    {
                        if (m_currentLengthIndex[i] < 1)
                        {
                            float l = m_chartData.m_lane[i].m_initialLength;
                            m_lanes[i].GetComponent<LaneHandler>().InitializeLength(m_chartData.m_lane[i].m_laneEventLength[m_currentLengthIndex[i]], l);

                            m_currentLengthIndex[i]++;
                        }
                        else
                        {
                            float l = m_chartData.m_lane[i].m_laneEventLength[m_currentLengthIndex[i] - 1].m_targetLength;

                            m_lanes[i].GetComponent<LaneHandler>().InitializeLength(m_chartData.m_lane[i].m_laneEventLength[m_currentLengthIndex[i]], l);

                            m_currentLengthIndex[i]++;
                        }
                    }
                }
            }
        }

    }
}
