using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChartData contains all the information of the chart, as well as the lane/note information
/// </summary>
[System.Serializable]
public class ChartData
{    
    public string            m_trackAudioPath;            // The path of where the song is contained
    public string            m_trackCoverArtPath;
    public string            m_trackName;
    public string            m_trackArtist;
    public int               m_trackDifficulty;

    public float             m_trackOffset;
    public float             m_trackBPM;

    public List<LaneData>    m_lane;
}
