using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ChartLevelSelect : MonoBehaviour
{
    public string selectedChartPath;

    private string selectedTrackName;

    public static string levelPath;

    public int difficultySelected;
    public Text trackHiScore;

    private void Start()
    {
        SelectDifficulty(difficultySelected);
    }

    private void Update()
    {
        selectedChartPath = selectedChartPath.Replace("_difficulty_", "_" + difficultySelected.ToString() + "_");
        selectedChartPath = selectedChartPath.Replace("_0_", "_" + difficultySelected.ToString() + "_");
        selectedChartPath = selectedChartPath.Replace("_1_", "_" + difficultySelected.ToString() + "_");
        selectedChartPath = selectedChartPath.Replace("_2_", "_" + difficultySelected.ToString() + "_");

        trackHiScore.text = PlayerPrefs.GetInt(selectedTrackName + "_" + difficultySelected + "_" + "HiScore").ToString("0000000");

        levelPath = selectedChartPath;
    }

    public void SelectTrackName(string _name)
    {
        selectedTrackName = _name;
    }

    public void SelectPath(string _path)
    {
        selectedChartPath = _path;
    }

    public void SelectDifficulty(int _difficulty)
    {
        difficultySelected = _difficulty;

        selectedChartPath = selectedChartPath.Replace("_difficulty_", "_" + difficultySelected.ToString() + "_");
        selectedChartPath = selectedChartPath.Replace("_0_", "_" + difficultySelected.ToString() + "_");
        selectedChartPath = selectedChartPath.Replace("_1_", "_" + difficultySelected.ToString() + "_");
        selectedChartPath = selectedChartPath.Replace("_2_", "_" + difficultySelected.ToString() + "_");
    }
}
