using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;
using TMPro;

/// <summary>
/// Holds information on the chart being edited, saving it as a JSON File
/// </summary>
public class LevelEditorManager : MonoBehaviour
{
    public static LevelEditorManager Instance;

    //----------------------------
    // Chart information
    //----------------------------
    public ChartData m_chartData;

    // Prefab of note
    public GameObject m_notePrefab;
    // Prefab of lane
    public GameObject m_lanePrefab;

    public GameObject m_laneIDDisplayPrefab;

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
        m_chartData = new ChartData();
        m_audioSource = GetComponent<AudioSource>();
    }

    public void ConfirmNewChart()
    {
        string filePath = EditorUtility.OpenFolderPanel("Save chart", "C:\\Unity Projects\\ShushokuSakuhin\\Assets\\Custom Assets\\Resources", "");
        string json = JsonUtility.ToJson(m_chartData, true);

        File.WriteAllText(filePath + "/" + m_chartData.m_trackName + "_Chart.txt", json);
    }

    private void Update()
    {
        m_chartData = ChartMetadataBuilder.m_chartData;
        m_secPerBeat = 60.0f / m_chartData.m_trackBPM;
        m_audioSource.clip = Resources.Load<AudioClip>(m_chartData.m_trackAudioPath);
        if (m_initialized)
        {
            TrackUpdate();
            LaneEventSpawn();
        }
    }
    public void Initialize()
    {
        // Initialize lanes and index array
        {
            m_chartData.m_lane = new List<LaneData>();

            m_totalLanes = 0;
            m_lanes = new List<GameObject>();
            m_nextNoteIndex = new List<int>();
            m_currentMovementIndex = new List<int>();
            m_currentRotationIndex = new List<int>();
            m_currentFadeIndex = new List<int>();
            m_currentLengthIndex = new List<int>();
        }

        m_audioSource.Play();

        m_initialized = true;
    }

    public void ChartOpened()
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
        }

        m_initialized = true;

        m_audioSource.Play();
    }
    public void InitLoadedLane(LaneData _laneData)
    {
        m_lanes.Add(Instantiate(m_lanePrefab, _laneData.m_initialPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, _laneData.m_initialRotation))));

        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_identifier = m_totalLanes;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_movementStartPosition = _laneData.m_initialPosition;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_startRotation = _laneData.m_initialRotation;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_startAlpha = 1.0f;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_laneEventFade.m_targetAlpha = 1.0f;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_startLength = 10.0f;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_laneEventLength.m_targetLength = 10.0f;

        m_nextNoteIndex.Add(0);
        m_currentMovementIndex.Add(0);
        m_currentRotationIndex.Add(0);
        m_currentFadeIndex.Add(0);
        m_currentLengthIndex.Add(0);

        Instantiate(m_laneIDDisplayPrefab, m_lanes[m_totalLanes].transform).GetComponent<TextMeshPro>().text = m_totalLanes.ToString();
        m_totalLanes++;
    }

    public void InitEmptyLane(LaneData _laneData)
    {
        LaneData newLane = new LaneData();
        newLane = _laneData;

        newLane.m_laneEventsMovement = new List<LaneEventMovement>();
        newLane.m_laneEventsRotation = new List<LaneEventRotation>();
        newLane.m_laneEventFade = new List<LaneEventFade>();
        newLane.m_notes = new List<NoteData>();

        m_lanes.Add(Instantiate(m_lanePrefab, newLane.m_initialPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, newLane.m_initialRotation))));

        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_identifier = m_totalLanes;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_movementStartPosition = newLane.m_initialPosition;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_startRotation = newLane.m_initialRotation;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_startAlpha = 1.0f;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_laneEventFade.m_targetAlpha = 1.0f;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_startLength = 10.0f;
        m_lanes[m_totalLanes].GetComponent<LaneHandler>().m_laneEventLength.m_targetLength = 10.0f;

        m_nextNoteIndex.Add(0);
        m_currentMovementIndex.Add(0);
        m_currentRotationIndex.Add(0);
        m_currentFadeIndex.Add(0);
        m_currentLengthIndex.Add(0);

        m_chartData.m_lane.Add(newLane);
        Instantiate(m_laneIDDisplayPrefab, m_lanes[m_totalLanes].transform).GetComponent<TextMeshPro>().text = m_totalLanes.ToString();
        m_totalLanes++;
    }

    private void TrackUpdate()
    {
        // How many seconds since we started
        m_trackPos = (float)(m_audioSource.time - m_chartData.m_trackOffset);
        // How many beats since we started
        m_trackPosInBeats = m_trackPos / m_secPerBeat;

        // Sort the lists so its nice and tidy
        for (int i = 0; i < m_chartData.m_lane.Count; i++ )
        {
            m_chartData.m_lane[i].m_laneEventsMovement = m_chartData.m_lane[i].m_laneEventsMovement.OrderBy(lst => lst.m_beat).ToList();
            m_chartData.m_lane[i].m_laneEventsRotation = m_chartData.m_lane[i].m_laneEventsRotation.OrderBy(lst => lst.m_beat).ToList();
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

        for (int i = 0; i < m_chartData.m_lane.Count; i++)
        {
            // For rewinding purposes we need to revert the index back to preview the proper movement in order. It's janky as shit.
            if (m_chartData.m_lane[i].m_laneEventsMovement.Count > 0 && m_currentMovementIndex[i] - 1 >= 0)
            {
                if (m_chartData.m_lane[i].m_laneEventsMovement[m_currentMovementIndex[i] - 1].m_beat + m_chartData.m_lane[i].m_laneEventsMovement[m_currentMovementIndex[i] - 1].m_duration >= m_trackPosInBeats)
                {
                    if (m_currentMovementIndex[i] - 1 >= 0)
                    {
                        m_currentMovementIndex[i]--;
                        // Continue looping but stop here since this means we are at 0, and therefore continuing through this specific iteration will shit itself cuz negative indexes suck
                        continue;
                    }

                    m_lanes[i].GetComponent<LaneHandler>().InitializeMovement(m_chartData.m_lane[i].m_laneEventsMovement[m_currentMovementIndex[i]], m_chartData.m_lane[i].m_laneEventsMovement[m_currentMovementIndex[i] - 1].m_targetPosition);
                }
            }
            else if (m_chartData.m_lane[i].m_laneEventsMovement.Count > 0 && m_currentMovementIndex[i] - 1 < 0)
            {
                m_lanes[i].GetComponent<LaneHandler>().InitializeMovement(m_chartData.m_lane[i].m_laneEventsMovement[m_currentMovementIndex[i]], m_chartData.m_lane[i].m_initialPosition);

            }

            // For rotation
            if (m_chartData.m_lane[i].m_laneEventsRotation.Count > 0 && m_currentRotationIndex[i] - 1 >= 0)
            {
                if (m_chartData.m_lane[i].m_laneEventsRotation[m_currentRotationIndex[i] - 1].m_beat + m_chartData.m_lane[i].m_laneEventsRotation[m_currentRotationIndex[i] - 1].m_duration >= m_trackPosInBeats)
                {
                    if (m_currentRotationIndex[i] - 1 >= 0)
                    {
                        m_currentRotationIndex[i]--;
                        // Continue looping but stop here since this means we are at 0, and therefore continuing through this specific iteration will be sad at itself cuz negative indexes are not fun
                        continue;
                    }

                    float z = m_chartData.m_lane[i].m_laneEventsRotation[m_currentRotationIndex[i] - 1].m_targetRotation;

                    if (z >= 360)
                    {
                        z %= 360;
                    }

                    m_lanes[i].GetComponent<LaneHandler>().InitializeRotation(m_chartData.m_lane[i].m_laneEventsRotation[m_currentRotationIndex[i]], z);
                }
            }
            else if (m_chartData.m_lane[i].m_laneEventsRotation.Count > 0 && m_currentRotationIndex[i] - 1 < 0)
            {
                m_lanes[i].GetComponent<LaneHandler>().InitializeRotation(m_chartData.m_lane[i].m_laneEventsRotation[m_currentRotationIndex[i]], m_chartData.m_lane[i].m_initialRotation);

            }

            // For fades
            if (m_chartData.m_lane[i].m_laneEventFade.Count > 0 && m_currentFadeIndex[i] - 1 >= 0)
            {
                if (m_chartData.m_lane[i].m_laneEventFade[m_currentFadeIndex[i] - 1].m_beat + m_chartData.m_lane[i].m_laneEventFade[m_currentFadeIndex[i] - 1].m_duration >= m_trackPosInBeats)
                {
                    if (m_currentFadeIndex[i] - 1 >= 0)
                    {
                        m_currentFadeIndex[i]--;
                        // Continue looping but stop here since this means we are at 0, and therefore continuing through this specific iteration will be sad at itself cuz negative indexes are not fun
                        continue;
                    }

                    float a = m_chartData.m_lane[i].m_laneEventFade[m_currentFadeIndex[i] - 1].m_targetAlpha;

                    m_lanes[i].GetComponent<LaneHandler>().InitializeFade(m_chartData.m_lane[i].m_laneEventFade[m_currentFadeIndex[i]], a);
                }
            }
            else if (m_chartData.m_lane[i].m_laneEventFade.Count > 0 && m_currentFadeIndex[i] - 1 < 0)
            {
                m_lanes[i].GetComponent<LaneHandler>().InitializeFade(m_chartData.m_lane[i].m_laneEventFade[m_currentFadeIndex[i]], m_chartData.m_lane[i].m_initialAlpha);
            }

            // For length changes
            if (m_chartData.m_lane[i].m_laneEventLength.Count > 0 && m_currentLengthIndex[i] - 1 >= 0)
            {
                if (m_chartData.m_lane[i].m_laneEventLength[m_currentLengthIndex[i] - 1].m_beat + m_chartData.m_lane[i].m_laneEventLength[m_currentLengthIndex[i] - 1].m_duration >= m_trackPosInBeats)
                {
                    if (m_currentLengthIndex[i] - 1 >= 0)
                    {
                        m_currentLengthIndex[i]--;
                        // Continue looping but stop here since this means we are at 0, and therefore continuing through this specific iteration will be sad at itself cuz negative indexes are not fun
                        continue;
                    }

                    float l = m_chartData.m_lane[i].m_laneEventLength[m_currentLengthIndex[i] - 1].m_targetLength;

                    m_lanes[i].GetComponent<LaneHandler>().InitializeLength(m_chartData.m_lane[i].m_laneEventLength[m_currentLengthIndex[i]], l);
                }
            }
            else if (m_chartData.m_lane[i].m_laneEventLength.Count > 0 && m_currentLengthIndex[i] - 1 < 0)
            {
                m_lanes[i].GetComponent<LaneHandler>().InitializeLength(m_chartData.m_lane[i].m_laneEventLength[m_currentLengthIndex[i]], m_chartData.m_lane[i].m_initialLength);
            }
        }
    }
}
