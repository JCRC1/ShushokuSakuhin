using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator[] animatorsToTrigger;

    public float transitionTime;

    public int sceneIndex;

    public AudioClip soundEffect;

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
        StartCoroutine(LoadLevel(sceneIndex));

        GetComponent<AudioSource>().clip = soundEffect;
        GetComponent<AudioSource>().Play();
    }

    IEnumerator LoadLevel(int _levelIndex)
    {
        for (int i = 0; i < animatorsToTrigger.Length; i++)
        {
            animatorsToTrigger[i].enabled = true;
            animatorsToTrigger[i].SetTrigger("Start");
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(_levelIndex);
    }
}
