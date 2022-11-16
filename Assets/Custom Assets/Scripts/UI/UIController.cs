using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Animator[] UIAnimator;

    public GameObject ScoreDisplay;

    public Image trackImage;
    public Text[] trackName;
    public Text[] difficulty;
    public Text artistName;

    public Text perfectCount;
    public Text goodCount;
    public Text missCount;

    public Text maxCombo;

    public Text accuracy;

    public Text scoreThisTime;
    public Text hiScore;

    private void Start()
    {
        trackImage.sprite = Resources.Load<Sprite>(GameManager.Instance.chartData.trackCoverArtPath);
    }

    private void Update()
    {

        for (int i = 0; i < 2; i++)
        {
            trackName[i].text = GameManager.Instance.chartData.trackName;

            if (GameManager.Instance.chartData.trackDifficulty == 0)
            {
                difficulty[i].text = "Easy";
            } 
            else if (GameManager.Instance.chartData.trackDifficulty == 1)
            {
                difficulty[i].text = "Normal";
            } 
            else if (GameManager.Instance.chartData.trackDifficulty == 2)
            {
                difficulty[i].text = "Hard";
            }
        }

        artistName.text = GameManager.Instance.chartData.trackArtist;

        // Hit Accuracy here
        perfectCount.text = ScoreController.Instance.perfectHitCount.ToString();
        goodCount.text = ScoreController.Instance.goodHitCount.ToString();
        missCount.text = ScoreController.Instance.missCount.ToString();

        maxCombo.text = ScoreController.Instance.maxCombo.ToString();

        accuracy.text = (((ScoreController.Instance.goodHitCount * 0.5f) + ScoreController.Instance.perfectHitCount) / GameManager.Instance.totalNotes * 100.0f).ToString("00.00") + "%";

        scoreThisTime.text = ScoreController.Instance.currentScore.ToString("0000000");

        // Check high score 
        if (ScoreController.Instance.currentScore > PlayerPrefs.GetInt(GameManager.Instance.chartData.trackName + "_" + GameManager.Instance.chartData.trackDifficulty + "_HiScore"))
        {
            PlayerPrefs.SetInt(GameManager.Instance.chartData.trackName + "_" + GameManager.Instance.chartData.trackDifficulty + "_HiScore", int.Parse(scoreThisTime.text));
        }

        hiScore.text = PlayerPrefs.GetInt(GameManager.Instance.chartData.trackName + "_" + GameManager.Instance.chartData.trackDifficulty + "_HiScore").ToString("0000000");

        if (GameManager.Instance.finalized)
        {
            for (int i = 0; i < UIAnimator.Length; i++)
            {
                UIAnimator[i].SetTrigger("Finalized");
            }

            ScoreDisplay.SetActive(true);
        }
    }
}
