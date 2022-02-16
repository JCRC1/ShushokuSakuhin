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
        var bp = new BrowserProperties();
        bp.initialDir = Application.dataPath + "\\Resources\\";
        bp.filter = "mp3 files (*.mp3)|*.mp3|wav files (*.wav)|*.wav|All Files (*.*)|*.*";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Do something with path(string)
            string temp = path.Replace(Application.dataPath + "\\Resources\\", "");
            string temp2 = temp.Replace(".mp3", "");
            string temp3 = temp2.Replace(".wav", "");

            m_chartData.m_trackAudioPath = temp3;
        });
    }

    public void OpenChart()
    {
        var bp = new BrowserProperties();
        bp.initialDir = Application.dataPath + "\\Resources\\";
        bp.filter = "txt files (*.txt)|*.txt|All Files (*.*)|*.*";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Do something with path(string)
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, LevelEditorManager.Instance.m_chartData);
        });
    }

    public void TrackCoverPathLoad()
    {
        var bp = new BrowserProperties();
        bp.initialDir = Application.dataPath + "\\Resources\\";
        bp.filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg|All Files (*.*)|*.*";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Do something with path(string)
            string temp = path.Replace(Application.dataPath + "\\Resources\\", "");
            string temp2 = temp.Replace(".png", "");
            string temp3 = temp2.Replace(".jpg", "");

            m_chartData.m_trackCoverArtPath = temp3;
        });
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
