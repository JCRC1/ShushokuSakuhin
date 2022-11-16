using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChartData contains all the information of the chart, as well as the lane/note information
/// </summary>
[System.Serializable]
public class ChartData
{    
    public string            trackAudioPath;            // The path of where the song is contained
    public string            trackCoverArtPath;
    public string            trackName;
    public string            trackArtist;
    public int               trackDifficulty;

    public float             trackOffset;
    public float             trackBPM;

    public List<LaneData>    lane;
}
