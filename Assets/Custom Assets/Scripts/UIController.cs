using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Animator[] m_UIAnimator;

    public GameObject m_ScoreDisplay;

    public Image m_trackImage;
    public Text[] m_trackName;
    public Text[] m_difficulty;
    public Text m_artistName;

    public Text m_perfectCount;
    public Text m_goodCount;
    public Text m_missCount;

    public Text m_maxCombo;

    public Text m_accuracy;

    public Text m_scoreThisTime;
    public Text m_hiScore;

    private void Start()
    {
        m_trackImage.sprite = Resources.Load<Sprite>(GameManager.Instance.m_chartData.m_trackCoverArtPath);
    }

    private void Update()
    {

        for (int i = 0; i < 2; i++)
        {
            m_trackName[i].text = GameManager.Instance.m_chartData.m_trackName;

            if (GameManager.Instance.m_chartData.m_trackDifficulty == 0)
            {
                m_difficulty[i].text = "Easy";
            } 
            else if (GameManager.Instance.m_chartData.m_trackDifficulty == 1)
            {
                m_difficulty[i].text = "Normal";
            } 
            else if (GameManager.Instance.m_chartData.m_trackDifficulty == 2)
            {
                m_difficulty[i].text = "Hard";
            }
        }

        m_artistName.text = GameManager.Instance.m_chartData.m_trackArtist;

        // Hit Accuracy here
        m_perfectCount.text = ScoreController.Instance.m_perfectHitCount.ToString();
        m_goodCount.text = ScoreController.Instance.m_goodHitCount.ToString();
        m_missCount.text = ScoreController.Instance.m_missCount.ToString();

        m_maxCombo.text = ScoreController.Instance.m_maxCombo.ToString();

        m_accuracy.text = (((ScoreController.Instance.m_goodHitCount * 0.5f) + ScoreController.Instance.m_perfectHitCount) / GameManager.Instance.m_totalNotes * 100.0f).ToString("00.00") + "%";

        if (GameManager.Instance.m_finalized)
        {
            for (int i = 0; i < m_UIAnimator.Length; i++)
            {
                m_UIAnimator[i].SetTrigger("Finalized");
            }

            m_ScoreDisplay.SetActive(true);
        }
    }
}
