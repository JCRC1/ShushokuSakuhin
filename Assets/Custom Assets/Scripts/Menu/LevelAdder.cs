using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class LevelAdder : MonoBehaviour
{
    public int chartFolderCount;

    public GameObject levelTabsContainer; // Where the level button will be
    public GameObject levelTabTemplate;

    public GameObject levelInfoContainer; // Where the level info will be
    public GameObject levelInfoTemplate;

    public ChartLevelSelect chartLevelSelect;

    private void Start()
    {
        // Check how many folders in this directory, each is a chart folder
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "\\Resources\\");
        chartFolderCount = dir.GetDirectories().Length;

        // Iterate through the chart folders
        for (int i = 0; i < chartFolderCount; i++)
        {
            // Check if the chart folder has all necessary files needed to create the objects
            DirectoryInfo chartDir = dir.GetDirectories()[i];
            if (chartDir.GetFiles().Length < 6)
            {
                continue;
            }

            // Now with those folders in mind, lets create some objects in the world
            // First the level select button tab
            {
                GameObject levelTab = Instantiate(levelTabTemplate, levelTabsContainer.transform);
                AudioClip trackPreview = Resources.Load<AudioClip>(chartDir.GetFiles("*Preview*")[0].FullName.Replace(Application.dataPath.Replace("/", "\\") + "\\Resources\\", "").Replace(".mp3", "").Replace(".wav", ""));
                Text levelName = levelTab.transform.GetChild(0).GetComponent<Text>();

                levelTab.SetActive(true);                                                       // Make Active
                levelName.text = chartDir.Name;                                                 // Name of the song

                // Pass to Select Path on the level select
                levelTab.GetComponent<Button>().onClick.AddListener(
                    delegate
                    {
                        chartLevelSelect.SelectPath(chartDir.Name + "\\" + chartDir.Name + "_difficulty_Chart.txt");
                    }
                    );

                // Pass to Select Track Name on the level select
                levelTab.GetComponent<Button>().onClick.AddListener(
                   delegate
                   {
                       chartLevelSelect.SelectTrackName(chartDir.Name);
                   }
                   );

                // Pass to BGM Checker
                levelTab.GetComponent<Button>().onClick.AddListener(
                   delegate
                   {
                       GetComponent<BGMFaderChecker>().SwapMusic(trackPreview);
                   }
                   );
            }

            // Next the level info display
            {
                GameObject levelInfo = Instantiate(levelInfoTemplate, levelInfoContainer.transform);
                Image trackImage = levelInfo.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();

                trackImage.sprite = Resources.Load<Sprite>(chartDir.GetFiles("*Image*")[0].FullName.Replace(Application.dataPath.Replace("/", "\\") + "\\Resources\\", "").Replace(".png", "").Replace(".jpg", ""));

                // Peek into a chart txt to get Artist name, this is probably spaghetti
                foreach (string line in File.ReadLines(chartDir.GetFiles("*0_Chart.txt")[0].FullName))
                {
                    if (line.Contains("Artist"))
                    {
                        string temp = line.Replace("    \"trackArtist\": ", "");
                        levelInfo.transform.GetChild(1).GetComponent<Text>().text = temp.Trim('\"').Trim(',').TrimEnd('\"');
                        continue;
                    }
                }

                // Add these to the objects to swap for the tabs
                GetComponent<TabGroup>().objectsToSwap.Add(levelInfo);
            }
        }
    }
}
