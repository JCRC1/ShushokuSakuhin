using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ChartLevelSelect : MonoBehaviour
{
    public string m_selectedChartPath;

    private string m_selectedTrackName;

    public static string m_levelPath;

    public int m_difficultySelected;
    public Text m_trackHiScore;

    private void Start()
    {
        SelectDifficulty(m_difficultySelected);
    }

    private void Update()
    {
        m_selectedChartPath = m_selectedChartPath.Replace("_difficulty_", "_" + m_difficultySelected.ToString() + "_");
        m_selectedChartPath = m_selectedChartPath.Replace("_0_", "_" + m_difficultySelected.ToString() + "_");
        m_selectedChartPath = m_selectedChartPath.Replace("_1_", "_" + m_difficultySelected.ToString() + "_");
        m_selectedChartPath = m_selectedChartPath.Replace("_2_", "_" + m_difficultySelected.ToString() + "_");

        m_trackHiScore.text = PlayerPrefs.GetInt(m_selectedTrackName + "_" + m_difficultySelected + "_" + "HiScore").ToString("0000000");

        m_levelPath = m_selectedChartPath;
    }

    public void SelectTrackName(string _name)
    {
        m_selectedTrackName = _name;
    }

    public void SelectPath(string _path)
    {
        m_selectedChartPath = _path;
    }

    public void SelectDifficulty(int _difficulty)
    {
        m_difficultySelected = _difficulty;

        m_selectedChartPath = m_selectedChartPath.Replace("_difficulty_", "_" + m_difficultySelected.ToString() + "_");
        m_selectedChartPath = m_selectedChartPath.Replace("_0_", "_" + m_difficultySelected.ToString() + "_");
        m_selectedChartPath = m_selectedChartPath.Replace("_1_", "_" + m_difficultySelected.ToString() + "_");
        m_selectedChartPath = m_selectedChartPath.Replace("_2_", "_" + m_difficultySelected.ToString() + "_");
    }
}
