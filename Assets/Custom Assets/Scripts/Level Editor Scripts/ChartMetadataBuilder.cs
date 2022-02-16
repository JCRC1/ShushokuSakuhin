using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using AnotherFileBrowser.Windows;

public class ChartMetadataBuilder : MonoBehaviour
{
    public static ChartData m_chartData;

    private void Start()
    {
        m_chartData = new ChartData();
    }

    public void TrackPathLoad()
    {
#if UNITY_EDITOR
        string filePath = EditorUtility.OpenFilePanel("Load track", "C:\\Unity Projects\\ShushokuSakuhin\\Assets\\Custom Assets\\Resources", "");
        string temp = filePath.Replace("C:/Unity Projects/ShushokuSakuhin/Assets/Custom Assets/Resources/", "");
        string temp2 = temp.Replace(".mp3", "");

        m_chartData.m_trackAudioPath = temp2;
#endif
    }

    public void OpenChart()
    {
#if UNITY_EDITOR
        string filePath = EditorUtility.OpenFilePanel("Load track", "C:\\Unity Projects\\ShushokuSakuhin\\Assets\\Custom Assets\\Resources", "");        

        string json = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(json, LevelEditorManager.Instance.m_chartData);
#endif
    }

    public void TrackCoverPathLoad()
    {
#if UNITY_EDITOR
        string filePath = EditorUtility.OpenFilePanel("Load track", "C:\\Unity Projects\\ShushokuSakuhin\\Assets\\Custom Assets\\Resources", "");
        string temp = filePath.Replace("C:/Unity Projects/ShushokuSakuhin/Assets/Custom Assets/Resources/", "");
        string temp2 = "";

        if (temp.Contains(".png"))
        {
            temp2 = temp.Replace(".png", "");
        } 
        else if (temp.Contains(".jpg"))
        {
            temp2 = temp.Replace(".jpg", "");
        }

        m_chartData.m_trackCoverArtPath = temp2;
#endif
    }

    public void TrackNameAdd(Text name)
    {
        m_chartData.m_trackName = name.text;
    }

    public void TrackArtistAdd(Text name)
    {
        m_chartData.m_trackArtist = name.text;
    }

    public void TrackDifficultyAdd(Dropdown dropdown)
    {
        m_chartData.m_trackDifficulty = dropdown.value;
    }

    public void TrackBPMAdd(Text bpm)
    {
        float.TryParse(bpm.text, out m_chartData.m_trackBPM);
    }

    public void TrackOffsetAdd(Text offset)
    {
        float.TryParse(offset.text, out m_chartData.m_trackOffset);
    }
}
