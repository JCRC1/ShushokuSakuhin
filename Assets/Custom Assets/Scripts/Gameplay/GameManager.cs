using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton GameManager
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2 /*Make sure we only ever do this check in the Game Scene*/)
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

    [Header("GameState")]
    public GameState gameState;

    public static event Action<GameState> OnGameStateChanged;

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
        ChangeGameState(GameState.GameState_Init);
    }

    private void Update()
    {
        UpdateGameState();
    }

    private void GameInit()
    {
        LoadChart();
        InitLanes();
        InitPlay();

        ChangeGameState(GameState.GameState_Update);
    }

    private void InitPlay()
    {
        audioSource = GetComponent<AudioSource>();
        secPerBeat = 60.0f / chartData.trackBPM;

        audioSource.clip = Resources.Load<AudioClip>(chartData.trackAudioPath);
        dspSongTime = (float)AudioSettings.dspTime;
        audioSource.Play();
        Invoke("ChartEnd", audioSource.clip.length);        // <<<< Gonna need to find a way to make this play better with the game states, maybe use a coroutine?
    }

    private void InitLanes()
    {
        // Initialize lanes and index array
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

    private void LoadChart()
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
        // This is to keep track of BPM Changes
        // Currently BPM is set on start
        // secPerBeat = 60.0f / chartData.trackBPM;

        // How many seconds since we started
        trackPos = (float)(AudioSettings.dspTime - dspSongTime - chartData.trackOffset);
        // How many beats since we started
        trackPosInBeats = trackPos / secPerBeat;

        if (trackPosInBeats >= (completedLoops + 1) * beatsPerLoop)
        {
            completedLoops++;
        }
        loopPosInBeats = trackPosInBeats - completedLoops * beatsPerLoop;
        loopPosInAnalog = loopPosInBeats / beatsPerLoop;
    }

    private void LaneEventSpawn()
    {
        for (int i = 0; i < chartData.lane.Count; i++)
        {
            MovementEventSpawn(i);
            RotationEventSpawn(i);
            FadeEventSpawn(i);
            LengthChangeEventSpawn(i);
        }
    }

    private void LengthChangeEventSpawn(int index)
    {
        // Length
        if (currentLengthIndex[index] <= 0)
        {
            currentLengthIndex[index] = 0;
        }

        // First check if the current index is less than the total count of events
        if (currentLengthIndex[index] < chartData.lane[index].laneEventLength.Count)
        {
            if (chartData.lane[index].laneEventLength[currentLengthIndex[index]].beat < trackPosInBeats)
            {
                if (currentLengthIndex[index] < 1)
                {
                    float l = chartData.lane[index].initialLength;
                    lanes[index].GetComponent<LaneHandler>().InitializeLength(chartData.lane[index].laneEventLength[currentLengthIndex[index]], l);

                    currentLengthIndex[index]++;
                }
                else
                {
                    float l = chartData.lane[index].laneEventLength[currentLengthIndex[index] - 1].targetLength;

                    lanes[index].GetComponent<LaneHandler>().InitializeLength(chartData.lane[index].laneEventLength[currentLengthIndex[index]], l);

                    currentLengthIndex[index]++;
                }
            }
        }
    }

    private void FadeEventSpawn(int index)
    {
        // Fade
        if (currentFadeIndex[index] <= 0)
        {
            currentFadeIndex[index] = 0;
        }

        // First check if the current index is less than the total count of events
        if (currentFadeIndex[index] < chartData.lane[index].laneEventFade.Count)
        {
            if (chartData.lane[index].laneEventFade[currentFadeIndex[index]].beat < trackPosInBeats)
            {
                if (currentFadeIndex[index] < 1)
                {
                    float a = chartData.lane[index].initialAlpha;
                    lanes[index].GetComponent<LaneHandler>().InitializeFade(chartData.lane[index].laneEventFade[currentFadeIndex[index]], a);

                    currentFadeIndex[index]++;
                }
                else
                {
                    float a = chartData.lane[index].laneEventFade[currentFadeIndex[index] - 1].targetAlpha;

                    if (a >= 1)
                        a = 1;
                    else if (a <= 0)
                        a = 0;

                    lanes[index].GetComponent<LaneHandler>().InitializeFade(chartData.lane[index].laneEventFade[currentFadeIndex[index]], a);

                    currentFadeIndex[index]++;
                }
            }
        }
    }

    private void RotationEventSpawn(int index)
    {
        if (currentRotationIndex[index] <= 0)
        {
            currentRotationIndex[index] = 0;
        }

        // First check if the current index is less than the total count of events
        if (currentRotationIndex[index] < chartData.lane[index].laneEventsRotation.Count)
        {
            if (chartData.lane[index].laneEventsRotation[currentRotationIndex[index]].beat < trackPosInBeats)
            {
                if (currentRotationIndex[index] < 1)
                {
                    float z = chartData.lane[index].initialRotation;
                    lanes[index].GetComponent<LaneHandler>().InitializeRotation(chartData.lane[index].laneEventsRotation[currentRotationIndex[index]], z);

                    currentRotationIndex[index]++;
                }
                else
                {
                    float z = chartData.lane[index].laneEventsRotation[currentRotationIndex[index] - 1].targetRotation;

                    if (z >= 360)
                    {
                        z %= 360;
                    }
                    lanes[index].GetComponent<LaneHandler>().InitializeRotation(chartData.lane[index].laneEventsRotation[currentRotationIndex[index]], z);

                    currentRotationIndex[index]++;
                }
            }
        }
    }

    private void MovementEventSpawn(int index)
    {
        // Make sure these don't go negative or else Unity has a cry
        if (currentMovementIndex[index] <= 0)
        {
            currentMovementIndex[index] = 0;
        }

        // First check if the current index is less than the total count of events
        if (currentMovementIndex[index] < chartData.lane[index].laneEventsMovement.Count)
        {
            if (chartData.lane[index].laneEventsMovement[currentMovementIndex[index]].beat < trackPosInBeats)
            {
                if (currentMovementIndex[index] < 1)
                {
                    lanes[index].GetComponent<LaneHandler>().InitializeMovement(chartData.lane[index].laneEventsMovement[currentMovementIndex[index]], chartData.lane[index].initialPosition);

                    currentMovementIndex[index]++;
                }
                else
                {
                    lanes[index].GetComponent<LaneHandler>().InitializeMovement(chartData.lane[index].laneEventsMovement[currentMovementIndex[index]], chartData.lane[index].laneEventsMovement[currentMovementIndex[index] - 1].targetPosition);

                    currentMovementIndex[index]++;
                }
            }
        }
    }

    private void ChangeGameState(GameState newState)
    {
        gameState = newState;
        OnGameStateChanged?.Invoke(newState);   // Invoke all functions subscribed to this event if any
    }

    private void UpdateGameState()
    {
        switch (gameState)
        {
            case GameState.GameState_Init:
                GameInit();
                break;

            case GameState.GameState_Update:
                GameUpdate();
                break;

            case GameState.GameState_Fin:
                GameFin();
                break;

            case GameState.GameState_Pause:
                break;
        }
    }

    private void GameFin()
    {
        for (int i = 0; i < lanes.Count; i++)
        {
            lanes[i].SetActive(false);
            audioSource.Stop();
        }

        finalized = true;
        initialized = false;
    }

    private void GameUpdate()
    {
        TrackUpdate();
        LaneEventSpawn();
    }

    private void ChartEnd()
    {
        ChangeGameState(GameState.GameState_Fin);
    }
}