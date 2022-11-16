using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance;

    public float currentScore;

    public int currentCombo;
    public int maxCombo;

    public int perfectHitCount;
    public int goodHitCount;
    public int missCount;

    public Text UIScore;
    public Text UICombo;

    public float scorePerNote;

    private float scorePerPerfect;
    private float scorePerGood;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(GameManager.Instance.totalNotes > 0)
            scorePerNote = 1000000.000f / GameManager.Instance.totalNotes;

        scorePerPerfect = scorePerNote;
        scorePerGood = scorePerNote * 0.5f;
    }

    private void Update()
    {
        if (!GameManager.Instance.finalized)
        {
            UIScore.text = Mathf.RoundToInt(currentScore).ToString("0000000");
            UICombo.text = currentCombo.ToString();
            if (currentCombo > 2)
            {
                UICombo.gameObject.SetActive(true);
            }
            else
            {
                UICombo.gameObject.SetActive(false);
            }
            currentScore = scorePerPerfect * perfectHitCount + scorePerGood * goodHitCount;

            // Check max combo
            if (currentCombo > maxCombo)
            {
                maxCombo = currentCombo;
            }
        }
    }

    public void AddGoodHit()
    {
        if (UICombo.gameObject.activeSelf)
        {
            UICombo.GetComponent<Animator>().SetTrigger("ComboUp");
        }
        goodHitCount++;
        currentCombo++;
    }

    public void AddPerfectHit()
    {
        if (UICombo.gameObject.activeSelf)
        {
            UICombo.GetComponent<Animator>().SetTrigger("ComboUp");
        }
        perfectHitCount++;
        currentCombo++;
    }
}
