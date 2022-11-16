using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject fadeObj;
    public GameObject countDownObj;

    public bool isPause;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
            {
                Pause();
            }
            else
            {
                countDownObj.GetComponent<Animator>().Play("CountdownResume");
                Invoker.InvokeDelayed(PanelResume, 2);
                Invoker.InvokeDelayed(Unpause, 4);
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameManager.Instance.audioSource.Pause();
        pausePanel.SetActive(true);
        AudioListener.pause = true;
        isPause = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        GameManager.Instance.audioSource.UnPause();
        pausePanel.SetActive(false);
        AudioListener.pause = false;
        isPause = false;
    }

    private void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioListener.pause = false;
    }

    public void FadeInToRestart()
    {
        fadeObj.GetComponent<Animator>().SetTrigger("Start");
        Invoker.InvokeDelayed(Restart, 2);
    }

    public void FadeInToMenu()
    {
        fadeObj.GetComponent<Animator>().SetTrigger("Start");
        Invoker.InvokeDelayed(ToMenu, 2);
    }

    public void CountDownToUnpause()
    {
        countDownObj.GetComponent<Animator>().Play("CountdownResume");
        Invoker.InvokeDelayed(PanelResume, 2);
        Invoker.InvokeDelayed(Unpause, 4);
    }

    private void PanelResume()
    {
        pausePanel.GetComponent<Animator>().SetTrigger("Resume");
    }

    private void ToMenu()
    {
        Time.timeScale = 1;
        PreviewTrack.Instance.ReturnToDefault();
        SceneManager.LoadScene(1);
        AudioListener.pause = false;
    }
}
