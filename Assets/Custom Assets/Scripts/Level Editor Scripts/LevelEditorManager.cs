using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;
using AnotherFileBrowser.Windows;
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
    public ChartData chartData;

    // Prefab of note
    public GameObject notePrefab;
    // Prefab of lane
    public GameObject lanePrefab;

    public GameObject laneIDDisplayPrefab;

    [HideInInspector]
    public List<GameObject> lanes;

    // Indexes
    [HideInInspector]
    public List<int> nextNoteIndex;
    //[HideInInspector]
    public List<int> currentMovementIndex;
    //[HideInInspector]
    public List<int> currentRotationIndex;
    //[HideInInspector]
    public List<int> currentFadeIndex;
    //[HideInInspector]
    public List<int> currentLengthIndex;

    // Total lanes
    [HideInInspector]
    public int totalLanes;
    // Beats to show in advance
    public float beatsToShow = 4;
    // Seconds between each beat
    [HideInInspector]
    public float secPerBeat;
    // Position of track in seconds
    public float trackPos;
    // Position of track in beats
    [HideInInspector]
    public float trackPosInBeats;

    //the number of beats in each loop
    public float beatsPerLoop;
    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;
    //The current position of the song within the loop in beats.
    public float loopPosInBeats;
    //The current relative position of the song within the loop measured between 0 and 1.
    public float loopPosInAnalog;

    // Attached AudioSource
    [HideInInspector]
    public AudioSource audioSource;

    public bool initialized;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        chartData = new ChartData();
        audioSource = GetComponent<AudioSource>();
    }

    public void ConfirmNewChart()
    {
        var bp = new BrowserProperties();
        bp.initialDir = "C:\\Unity Projects\\ShushokuSakuhin\\Assets\\Resources";
        bp.filter = "txt files (*.txt)|*.txt";
        bp.filterIndex = 0;

        new FileBrowser().SaveFileBrowser(bp, chartData.trackName + "_" + chartData.trackDifficulty + "_Chart.txt", ".txt", path =>
        {
            //Do something with path(string)
            string json = JsonUtility.ToJson(chartData, true);
            File.WriteAllText(path, json);
        });
    }

    private void Update()
    {
        chartData = ChartMetadataBuilder.chartData;
        secPerBeat = 60.0f / chartData.trackBPM;
        audioSource.clip = Resources.Load<AudioClip>(chartData.trackAudioPath);
        if (initialized)
        {
            TrackUpdate();
            LaneEventSpawn();
        }
    }
    public void Initialize()
    {
        // Initialize lanes and index array
        {
            chartData.lane = new List<LaneData>();

            totalLanes = 0;
            lanes = new List<GameObject>();
            nextNoteIndex = new List<int>();
            currentMovementIndex = new List<int>();
            currentRotationIndex = new List<int>();
            currentFadeIndex = new List<int>();
            currentLengthIndex = new List<int>();
        }

        audioSource.Play();

        Resources.FindObjectsOfTypeAll<NoteListDisplay>()[0].Initialized();
        initialized = true;
    }

    public void ChartOpened()
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
        }

        initialized = true;
        EventListDisplay.Instance.GenerateLoadedList();
        Resources.FindObjectsOfTypeAll<NoteListDisplay>()[0].GenerateLoadedList();
        audioSource.Play();
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

        Instantiate(laneIDDisplayPrefab, lanes[totalLanes].transform).GetComponent<TextMeshPro>().text = totalLanes.ToString();
        totalLanes++;
    }

    public void InitEmptyLane(LaneData _laneData)
    {
        LaneData newLane = new LaneData();
        newLane = _laneData;

        newLane.laneEventsMovement = new List<LaneEventMovement>();
        newLane.laneEventsRotation = new List<LaneEventRotation>();
        newLane.laneEventFade = new List<LaneEventFade>();
        newLane.laneEventLength = new List<LaneEventLength>();
        newLane.singleNote = new List<SingleNoteData>();
        newLane.holdNote = new List<HoldNoteData>();

        lanes.Add(Instantiate(lanePrefab, newLane.initialPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, newLane.initialRotation))));

        lanes[totalLanes].GetComponent<LaneHandler>().identifier = totalLanes;
        lanes[totalLanes].GetComponent<LaneHandler>().movementStartPosition = newLane.initialPosition;
        lanes[totalLanes].GetComponent<LaneHandler>().laneEventMovement.targetPosition = newLane.initialPosition;

        lanes[totalLanes].GetComponent<LaneHandler>().startRotation = newLane.initialRotation;
        lanes[totalLanes].GetComponent<LaneHandler>().laneEventRotation.targetRotation = newLane.initialRotation;

        lanes[totalLanes].GetComponent<LaneHandler>().startAlpha = 1.0f;
        lanes[totalLanes].GetComponent<LaneHandler>().laneEventFade.targetAlpha = 1.0f;
        lanes[totalLanes].GetComponent<LaneHandler>().startLength = 10.0f;
        lanes[totalLanes].GetComponent<LaneHandler>().laneEventLength.targetLength = 10.0f;

        nextNoteIndex.Add(0);
        currentMovementIndex.Add(0);
        currentRotationIndex.Add(0);
        currentFadeIndex.Add(0);
        currentLengthIndex.Add(0);

        chartData.lane.Add(newLane);
        Instantiate(laneIDDisplayPrefab, lanes[totalLanes].transform).GetComponent<TextMeshPro>().text = totalLanes.ToString();
        totalLanes++;
    }

    private void TrackUpdate()
    {
        // How many seconds since we started
        trackPos = (float)(audioSource.time - chartData.trackOffset);
        // How many beats since we started
        trackPosInBeats = trackPos / secPerBeat;

        if (trackPosInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPosInBeats = trackPosInBeats - completedLoops * beatsPerLoop;

        loopPosInAnalog = loopPosInBeats / beatsPerLoop;

        // Sort the lists so its nice and tidy
        for (int i = 0; i < chartData.lane.Count; i++ )
        {
            chartData.lane[i].laneEventsMovement = chartData.lane[i].laneEventsMovement.OrderBy(lst => lst.beat).ToList();
            chartData.lane[i].laneEventsRotation = chartData.lane[i].laneEventsRotation.OrderBy(lst => lst.beat).ToList();
        }
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

        for (int i = 0; i < chartData.lane.Count; i++)
        {
            // For rewinding purposes we need to revert the index back to preview the proper movement in order. It's janky as shit.
            if (chartData.lane[i].laneEventsMovement.Count > 0 && currentMovementIndex[i] - 1 >= 0)
            {
                if (chartData.lane[i].laneEventsMovement[currentMovementIndex[i] - 1].beat + chartData.lane[i].laneEventsMovement[currentMovementIndex[i] - 1].duration >= trackPosInBeats)
                {
                    if (currentMovementIndex[i] - 1 >= 0)
                    {
                        currentMovementIndex[i]--;
                        // Continue looping but stop here since this means we are at 0, and therefore continuing through this specific iteration will shit itself cuz negative indexes suck
                        continue;
                    }

                    lanes[i].GetComponent<LaneHandler>().InitializeMovement(chartData.lane[i].laneEventsMovement[currentMovementIndex[i]], chartData.lane[i].laneEventsMovement[currentMovementIndex[i] - 1].targetPosition);
                }
            }
            else if (chartData.lane[i].laneEventsMovement.Count > 0 && currentMovementIndex[i] - 1 < 0)
            {
                lanes[i].GetComponent<LaneHandler>().InitializeMovement(chartData.lane[i].laneEventsMovement[currentMovementIndex[i]], chartData.lane[i].initialPosition);

            }

            // For rotation
            if (chartData.lane[i].laneEventsRotation.Count > 0 && currentRotationIndex[i] - 1 >= 0)
            {
                if (chartData.lane[i].laneEventsRotation[currentRotationIndex[i] - 1].beat + chartData.lane[i].laneEventsRotation[currentRotationIndex[i] - 1].duration >= trackPosInBeats)
                {
                    if (currentRotationIndex[i] - 1 >= 0)
                    {
                        currentRotationIndex[i]--;
                        // Continue looping but stop here since this means we are at 0, and therefore continuing through this specific iteration will be sad at itself cuz negative indexes are not fun
                        continue;
                    }

                    float z = chartData.lane[i].laneEventsRotation[currentRotationIndex[i] - 1].targetRotation;

                    if (z >= 360)
                    {
                        z %= 360;
                    }

                    lanes[i].GetComponent<LaneHandler>().InitializeRotation(chartData.lane[i].laneEventsRotation[currentRotationIndex[i]], z);
                }
            }
            else if (chartData.lane[i].laneEventsRotation.Count > 0 && currentRotationIndex[i] - 1 < 0)
            {
                lanes[i].GetComponent<LaneHandler>().InitializeRotation(chartData.lane[i].laneEventsRotation[currentRotationIndex[i]], chartData.lane[i].initialRotation);

            }

            // For fades
            if (chartData.lane[i].laneEventFade.Count > 0 && currentFadeIndex[i] - 1 >= 0)
            {
                if (chartData.lane[i].laneEventFade[currentFadeIndex[i] - 1].beat + chartData.lane[i].laneEventFade[currentFadeIndex[i] - 1].duration >= trackPosInBeats)
                {
                    if (currentFadeIndex[i] - 1 >= 0)
                    {
                        currentFadeIndex[i]--;
                        // Continue looping but stop here since this means we are at 0, and therefore continuing through this specific iteration will be sad at itself cuz negative indexes are not fun
                        continue;
                    }

                    float a = chartData.lane[i].laneEventFade[currentFadeIndex[i] - 1].targetAlpha;

                    lanes[i].GetComponent<LaneHandler>().InitializeFade(chartData.lane[i].laneEventFade[currentFadeIndex[i]], a);
                }
            }
            else if (chartData.lane[i].laneEventFade.Count > 0 && currentFadeIndex[i] - 1 < 0)
            {
                lanes[i].GetComponent<LaneHandler>().InitializeFade(chartData.lane[i].laneEventFade[currentFadeIndex[i]], chartData.lane[i].initialAlpha);
            }

            // For length changes
            if (chartData.lane[i].laneEventLength.Count > 0 && currentLengthIndex[i] - 1 >= 0)
            {
                if (chartData.lane[i].laneEventLength[currentLengthIndex[i] - 1].beat + chartData.lane[i].laneEventLength[currentLengthIndex[i] - 1].duration >= trackPosInBeats)
                {
                    if (currentLengthIndex[i] - 1 >= 0)
                    {
                        currentLengthIndex[i]--;
                        // Continue looping but stop here since this means we are at 0, and therefore continuing through this specific iteration will be sad at itself cuz negative indexes are not fun
                        continue;
                    }

                    float l = chartData.lane[i].laneEventLength[currentLengthIndex[i] - 1].targetLength;

                    lanes[i].GetComponent<LaneHandler>().InitializeLength(chartData.lane[i].laneEventLength[currentLengthIndex[i]], l);
                }
            }
            else if (chartData.lane[i].laneEventLength.Count > 0 && currentLengthIndex[i] - 1 < 0)
            {
                lanes[i].GetComponent<LaneHandler>().InitializeLength(chartData.lane[i].laneEventLength[currentLengthIndex[i]], chartData.lane[i].initialLength);
            }
        }
    }
}
