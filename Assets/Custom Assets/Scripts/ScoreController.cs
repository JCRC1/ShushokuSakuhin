using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance;

    public float m_currentScore;

    public int m_currentCombo;
    public int m_maxCombo;

    public int m_perfectHitCount;
    public int m_goodHitCount;
    public int m_missCount;

    public Text m_UIScore;
    public Text m_UICombo;

    public float m_scorePerNote;

    private float m_scorePerPerfect;
    private float m_scorePerGood;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(GameManager.Instance.m_totalNotes > 0)
            m_scorePerNote = 1000000.000f / GameManager.Instance.m_totalNotes;

        m_scorePerPerfect = m_scorePerNote;
        m_scorePerGood = m_scorePerNote * 0.5f;
    }

    private void Update()
    {
        m_UIScore.text = Mathf.RoundToInt(m_currentScore).ToString("0000000");
        m_UICombo.text = m_currentCombo.ToString();
        if (m_currentCombo > 2)
        {
            m_UICombo.gameObject.SetActive(true);
            m_UICombo.GetComponent<Animator>().SetTrigger("ComboUp");
        }
        else
        {
            m_UICombo.gameObject.SetActive(false);
        }
        m_currentScore = m_scorePerPerfect * m_perfectHitCount + m_scorePerGood * m_goodHitCount;

        // Check max combo
        if (m_currentCombo > m_maxCombo)
        {
            m_maxCombo = m_currentCombo;
        }
    }

    public void AddGoodHit()
    {
        if (m_UICombo.gameObject.activeSelf)
        {
            m_UICombo.GetComponent<Animator>().SetTrigger("ComboUp");
        }
        m_goodHitCount++;
        m_currentCombo++;
    }

    public void AddPerfectHit()
    {
        if (m_UICombo.gameObject.activeSelf)
        {
            m_UICombo.GetComponent<Animator>().SetTrigger("ComboUp");
        }
        m_perfectHitCount++;
        m_currentCombo++;
    }
}
