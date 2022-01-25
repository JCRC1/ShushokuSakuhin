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
    public ChartData m_chartData;

    // Prefab of note
    public GameObject m_notePrefab;
    // Prefab of lane
    public GameObject m_lanePrefab;

    [HideInInspector]
    public GameObject[] m_lanes;
    // Index of next note to spawn
    [HideInInspector]
    public int[] m_nextNoteIndex;
    // Index of next lane event (Movement)
    [HideInInspector]
    public int[] m_nextLaneMovementIndex;
    // Index of next lane event (Rotation)
    [HideInInspector]
    public int[] m_nextRotationIndex;
    // Beats to show in advance
    public float m_beatsToShow = 4;
    // Seconds between each beat
    [HideInInspector]
    public float m_secPerBeat;
    // Position of track in seconds
    public float m_trackPos;
    // Position of track in beats
    [HideInInspector]
    public float m_trackPosInBeats;
    // Seconds since start
    [HideInInspector]
    public float m_dspTrackTime;
    // Attached AudioSource
    [HideInInspector]
    public AudioSource m_audioSource;

    public bool m_initialized;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        string json = File.ReadAllText("C:/Unity Projects/ShushokuSakuhin/Assets/Custom Assets/Resources/Test_Chart/A_Chart.txt");
        JsonUtility.FromJsonOverwrite(json, m_chartData);
        Initialize();
    }

    private void Update()
    {
        TrackUpdate();
        LaneEventSpawn();
    }

    private void Initialize()
    {
        // Initialize lanes and index array
        {
            m_lanes = new GameObject[m_chartData.m_lane.Count];
            m_nextNoteIndex = new int[m_chartData.m_lane.Count];
            m_nextLaneMovementIndex = new int[m_chartData.m_lane.Count];
            m_nextRotationIndex = new int[m_chartData.m_lane.Count];

            for (int i = 0; i < m_chartData.m_lane.Count; i++)
            {
                m_lanes[i] = Instantiate(m_lanePrefab, m_chartData.m_lane[i].m_initialPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, m_chartData.m_lane[i].m_initialRotation)));
                m_lanes[i].GetComponent<LaneHandler>().m_identifier = i;
                m_nextNoteIndex[i] = 0;
                m_nextLaneMovementIndex[i] = 0;
                m_nextRotationIndex[i] = 0;
            }
        }

        m_audioSource = GetComponent<AudioSource>();

        m_secPerBeat = 60.0f / m_chartData.m_trackBPM;

        m_dspTrackTime = (float)AudioSettings.dspTime;

        m_audioSource.clip = Resources.Load<AudioClip>(m_chartData.m_trackAudioPath);
        m_audioSource.Play();
        m_initialized = true;
    }     

    private void TrackUpdate()
    {
        // How many seconds since we started
        m_trackPos = (float)(AudioSettings.dspTime - m_dspTrackTime - m_chartData.m_trackOffset);
        // How many beats since we started
        m_trackPosInBeats = m_trackPos / m_secPerBeat;
    }

    private void LaneEventSpawn()
    {
        for (int i = 0; i < m_chartData.m_lane.Count; i++)
        {
            // Lane Events
            {
                // First check if the current index is less than the total count of events
                if (m_nextLaneMovementIndex[i] < m_chartData.m_lane[i].m_laneEventsMovement.Count)
                {
                    if (m_chartData.m_lane[i].m_laneEventsMovement[m_nextLaneMovementIndex[i]].m_beat < m_trackPosInBeats)
                    {
                        //m_lanes[i].GetComponent<LaneHandler>().InitializeMovement(m_chartData.m_lane[i].m_laneEventsMovement[m_nextLaneMovementIndex[i]]);
                        m_nextLaneMovementIndex[i]++;
                    }
                }
                // First check if the current index is less than the total count of events
                if (m_nextRotationIndex[i] < m_chartData.m_lane[i].m_laneEventsRotation.Count)
                {
                    if (m_chartData.m_lane[i].m_laneEventsRotation[m_nextRotationIndex[i]].m_beat < m_trackPosInBeats)
                    {
                        //m_lanes[i].GetComponent<LaneHandler>().InitializeRotation(m_chartData.m_lane[i].m_laneEventsRotation[m_nextRotationIndex[i]]);
                        m_nextRotationIndex[i]++;
                    }
                }
            }
        }
    }
}
