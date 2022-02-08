using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance;

    public float m_currentScore;

    public Text m_UIScore;

    public float m_scorePerNote;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(GameManager.Instance.m_totalNotes > 0)
            m_scorePerNote = 1000000.000f / GameManager.Instance.m_totalNotes;
    }

    private void Update()
    {
        m_UIScore.text = Mathf.RoundToInt(m_currentScore).ToString("0000000");
    }

    public void AddScoreOnHit()
    {
        m_currentScore += m_scorePerNote;
    }
}
