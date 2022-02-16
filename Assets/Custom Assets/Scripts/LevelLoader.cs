using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator[] m_animatorsToTrigger;

    public float m_transitionTime;

    public int m_sceneIndex;

    public AudioClip m_soundEffect;

    private void Update()
    {
        // If in the title scene
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                LoadNextLevel();
            }
        }
    }

    public virtual void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(m_sceneIndex));

        GetComponent<AudioSource>().clip = m_soundEffect;
        GetComponent<AudioSource>().Play();
    }

    IEnumerator LoadLevel(int _levelIndex)
    {
        for (int i = 0; i < m_animatorsToTrigger.Length; i++)
        {
            m_animatorsToTrigger[i].enabled = true;
            m_animatorsToTrigger[i].SetTrigger("Start");
        }

        yield return new WaitForSeconds(m_transitionTime);

        SceneManager.LoadScene(_levelIndex);
    }
}
