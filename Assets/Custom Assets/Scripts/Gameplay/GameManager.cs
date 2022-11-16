using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    // Singleton GameManager
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
            {
                Debug.LogError("GameManager is NULL");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError("There is already a GameManagerObject. There should only ever be one in the scene");
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    [HideInInspector] public ChartData chartData;
    [HideInInspector] public AudioSource audioSource;

    [Header("Chart Information")]
    [Tooltip("Path of chart from Resources")]
    public string path;
    [Tooltip("Toggle between using the loaded path from the menu or the one written on the editor")]
    public bool manualMode;

    [Header("Prefabs")]
    public GameObject lanePrefab;

    // Lane Information
    [HideInInspector] public List<GameObject> lanes;
    [HideInInspector] public int totalLanes;
    [HideInInspector] public int totalNotes;

    // Indexes, these are lists of intergers, each for the corresponding lane
    // These keep track of what is the current event/note index
    [HideInInspector] public List<int> nextNoteIndex;
    [HideInInspector] public List<int> currentMovementIndex;
    [HideInInspector] public List<int> currentRotationIndex;
    [HideInInspector] public List<int> currentFadeIndex;
    [HideInInspector] public List<int> currentLengthIndex;

    [Header("Timing Information")]
    [Tooltip("Beats ahead of the song that the notes should start spawning from")]
    public float beatsToShow = 4;
    [Tooltip("The window of time, in beats, for the player to hit the notes with PERFECT timing")]
    public float hitWindow;
    [Tooltip("This is for animation timing. The amount of beats one animation loop should last")]
    public float beatsPerLoop; 

    // Timing information
    [HideInInspector] public float secPerBeat;
    [HideInInspector] public float trackPos;            // In seconds
    [HideInInspector] public float trackPosInBeats;     // In beats
    private float dspSongTime;                          // Current timing of the song in dspTime


    // Animation timing information
    [HideInInspector] public int completedLoops = 0;    // The total number of loops completed since the looping clip first started
    [HideInInspector] public float loopPosInBeats;      //The current position of the song within the loop in beats.
    [HideInInspector] public float loopPosInAnalog;     //The current relative position of the song within the loop measured between 0 and 1.

    [HideInInspector] public bool initialized;
    [HideInInspector] public bool finalized;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (initialized)
        {
            TrackUpdate();
            LaneEventSpawn();
        }

        if (finalized)
        {
            for (int i = 0; i < lanes.Count; i++)
            {
                lanes[i].SetActive(false);
                audioSource.Stop();
            }
        }
    }

    private void Initialize()
    {
        chartData = new ChartData();
        string json;
        if (manualMode)
        {
            json = File.ReadAllText("C:/Unity Projects/ShushokuSakuhin/Assets/Resources/" + path);
        }
        else
        {
            json = File.ReadAllText("C:/Unity Projects/ShushokuSakuhin/Assets/Resources/" + ChartLevelSelect.levelPath);
        }
        JsonUtility.FromJsonOverwrite(json, chartData);

        // Initialize lanes and index array
        {
            totalLanes = 0;
            lanes = new List<GameObject>();
            nextNoteIndex = new List<int>();
            currentMovementIndex = new List<int>();
            currentRotationIndex = new List<int>();
            currentFadeIndex = new List<int>();
            currentLengthIndex = new List<int>();

        for (int i = 0; i < chartData.lane.Count; i++)
            {
                InitLoadedLane(chartData.lane[i]);

                totalNotes += chartData.lane[i].singleNote.Count + chartData.lane[i].holdNote.Count;
            }
        }

        audioSource = GetComponent<AudioSource>();

        secPerBeat = 60.0f / chartData.trackBPM;

        audioSource.clip = Resources.Load<AudioClip>(chartData.trackAudioPath);
        dspSongTime = (float)AudioSettings.dspTime;
        audioSource.Play();
        Invoke("EndGame", audioSource.clip.length);
        initialized = true;
    }

    public void InitLoadedLane(LaneData _laneData)
    {
        lanes.Add(Instantiate(lanePrefab, _laneData.initialPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, _laneData.initialRotation))));

        lanes[totalLanes].GetComponent<LaneHandler>().identifier = totalLanes;

        lanes[totalLanes].GetComponent<LaneHandler>().movementStartPosition = _laneData.initialPosition;
        lanes[totalLanes].GetComponent<LaneHandler>().laneEventMovement.targetPosition = _laneData.initialPosition;

        lanes[totalLanes].GetComponent<LaneHandler>().startRotation = _laneData.initialRotation;
        lanes[totalLanes].GetComponent<LaneHandler>().laneEventRotation.targetRotation = _laneData.initialRotation;

        lanes[totalLanes].GetComponent<LaneHandler>().startAlpha = 1.0f;
        lanes[totalLanes].GetComponent<LaneHandler>().laneEventFade.targetAlpha = 1.0f;
        lanes[totalLanes].GetComponent<LaneHandler>().startLength = 10.0f;
        lanes[totalLanes].GetComponent<LaneHandler>().laneEventLength.targetLength = 10.0f;

        nextNoteIndex.Add(0);
        currentMovementIndex.Add(0);
        currentRotationIndex.Add(0);
        currentFadeIndex.Add(0);
        currentLengthIndex.Add(0);

        totalLanes++;
    }

    private void TrackUpdate()
    {
        secPerBeat = 60.0f / chartData.trackBPM;
        // How many seconds since we started
        //trackPos = (float)(audioSource.time - chartData.trackOffset);

        trackPos = (float)(AudioSettings.dspTime - dspSongTime - chartData.trackOffset);
        // How many beats since we started
        trackPosInBeats = trackPos / secPerBeat;

        if (trackPosInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPosInBeats = trackPosInBeats - completedLoops * beatsPerLoop;

        loopPosInAnalog = loopPosInBeats / beatsPerLoop;
    }

    private void LaneEventSpawn()
    {
        for (int i = 0; i < chartData.lane.Count; i++)
        {
            // Lane Events
            {
                // Make sure these don't go negative or else Unity has a cry
                if (currentMovementIndex[i] <= 0)
                {
                    currentMovementIndex[i] = 0;
                }

                // First check if the current index is less than the total count of events
                if (currentMovementIndex[i] < chartData.lane[i].laneEventsMovement.Count)
                {
                    if (chartData.lane[i].laneEventsMovement[currentMovementIndex[i]].beat < trackPosInBeats)
                    {
                        if (currentMovementIndex[i] < 1)
                        {
                            lanes[i].GetComponent<LaneHandler>().InitializeMovement(chartData.lane[i].laneEventsMovement[currentMovementIndex[i]], chartData.lane[i].initialPosition);

                            currentMovementIndex[i]++;
                        }
                        else
                        {
                            lanes[i].GetComponent<LaneHandler>().InitializeMovement(chartData.lane[i].laneEventsMovement[currentMovementIndex[i]], chartData.lane[i].laneEventsMovement[currentMovementIndex[i] - 1].targetPosition);

                            currentMovementIndex[i]++;
                        }
                    }
                }

                if (currentRotationIndex[i] <= 0)
                {
                    currentRotationIndex[i] = 0;
                }

                // First check if the current index is less than the total count of events
                if (currentRotationIndex[i] < chartData.lane[i].laneEventsRotation.Count)
                {
                    if (chartData.lane[i].laneEventsRotation[currentRotationIndex[i]].beat < trackPosInBeats)
                    {
                        if (currentRotationIndex[i] < 1)
                        {
                            float z = chartData.lane[i].initialRotation;
                            lanes[i].GetComponent<LaneHandler>().InitializeRotation(chartData.lane[i].laneEventsRotation[currentRotationIndex[i]], z);

                            currentRotationIndex[i]++;
                        }
                        else
                        {
                            float z = chartData.lane[i].laneEventsRotation[currentRotationIndex[i] - 1].targetRotation;

                            if (z >= 360)
                            {
                                z %= 360;
                            }
                            lanes[i].GetComponent<LaneHandler>().InitializeRotation(chartData.lane[i].laneEventsRotation[currentRotationIndex[i]], z);

                            currentRotationIndex[i]++;
                        }
                    }
                }

                // Fade
                if (currentFadeIndex[i] <= 0)
                {
                    currentFadeIndex[i] = 0;
                }

                // First check if the current index is less than the total count of events
                if (currentFadeIndex[i] < chartData.lane[i].laneEventFade.Count)
                {
                    if (chartData.lane[i].laneEventFade[currentFadeIndex[i]].beat < trackPosInBeats)
                    {
                        if (currentFadeIndex[i] < 1)
                        {
                            float a = chartData.lane[i].initialAlpha;
                            lanes[i].GetComponent<LaneHandler>().InitializeFade(chartData.lane[i].laneEventFade[currentFadeIndex[i]], a);

                            currentFadeIndex[i]++;
                        }
                        else
                        {
                            float a = chartData.lane[i].laneEventFade[currentFadeIndex[i] - 1].targetAlpha;

                            if (a >= 1)
                                a = 1;
                            else if (a <= 0)
                                a = 0;

                            lanes[i].GetComponent<LaneHandler>().InitializeFade(chartData.lane[i].laneEventFade[currentFadeIndex[i]], a);

                            currentFadeIndex[i]++;
                        }
                    }
                }

                // Length
                if (currentLengthIndex[i] <= 0)
                {
                    currentLengthIndex[i] = 0;
                }

                // First check if the current index is less than the total count of events
                if (currentLengthIndex[i] < chartData.lane[i].laneEventLength.Count)
                {
                    if (chartData.lane[i].laneEventLength[currentLengthIndex[i]].beat < trackPosInBeats)
                    {
                        if (currentLengthIndex[i] < 1)
                        {
                            float l = chartData.lane[i].initialLength;
                            lanes[i].GetComponent<LaneHandler>().InitializeLength(chartData.lane[i].laneEventLength[currentLengthIndex[i]], l);

                            currentLengthIndex[i]++;
                        }
                        else
                        {
                            float l = chartData.lane[i].laneEventLength[currentLengthIndex[i] - 1].targetLength;

                            lanes[i].GetComponent<LaneHandler>().InitializeLength(chartData.lane[i].laneEventLength[currentLengthIndex[i]], l);

                            currentLengthIndex[i]++;
                        }
                    }
                }
            }
        }

    }

    private void EndGame()
    {
        finalized = true;
        initialized = false;
    }
}
