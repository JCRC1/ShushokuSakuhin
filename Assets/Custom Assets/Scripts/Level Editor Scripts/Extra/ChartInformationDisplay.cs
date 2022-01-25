using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartInformationDisplay : MonoBehaviour
{
    public InputField m_trackName;
    public InputField m_trackArtist;
    public InputField m_trackBPM;
    public InputField m_trackOffset;
    public Dropdown m_difficulty;

    public void DisplayChartInfo()
    {
        m_trackName.text = LevelEditorManager.Instance.m_chartData.m_trackName;
        m_trackArtist.text = LevelEditorManager.Instance.m_chartData.m_trackArtist;
        m_trackBPM.text = LevelEditorManager.Instance.m_chartData.m_trackBPM.ToString();
        m_trackOffset.text = LevelEditorManager.Instance.m_chartData.m_trackOffset.ToString("0.000");
        m_difficulty.value = LevelEditorManager.Instance.m_chartData.m_trackDifficulty;
    }
}
