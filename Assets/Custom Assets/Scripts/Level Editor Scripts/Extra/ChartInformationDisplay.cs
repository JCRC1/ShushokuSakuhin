using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartInformationDisplay : MonoBehaviour
{
    public InputField trackName;
    public InputField trackArtist;
    public InputField trackBPM;
    public InputField trackOffset;
    public Dropdown difficulty;

    public void DisplayChartInfo()
    {
        trackName.text = LevelEditorManager.Instance.chartData.trackName;
        trackArtist.text = LevelEditorManager.Instance.chartData.trackArtist;
        trackBPM.text = LevelEditorManager.Instance.chartData.trackBPM.ToString();
        trackOffset.text = LevelEditorManager.Instance.chartData.trackOffset.ToString("0.000");
        difficulty.value = LevelEditorManager.Instance.chartData.trackDifficulty;
    }
}
