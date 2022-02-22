using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject m_pausePanel;
    public GameObject m_fadeObj;
    public GameObject m_countDownObj;

    public bool m_isPause;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!m_isPause)
            {
                Pause();
            }
            else
            {
                m_countDownObj.GetComponent<Animator>().Play("CountdownResume");
                Invoker.InvokeDelayed(PanelResume, 2);
                Invoker.InvokeDelayed(Unpause, 4);
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameManager.Instance.m_audioSource.Pause();
        m_pausePanel.SetActive(true);
        AudioListener.pause = true;
        m_isPause = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        GameManager.Instance.m_audioSource.UnPause();
        m_pausePanel.SetActive(false);
        AudioListener.pause = false;
        m_isPause = false;
    }

    private void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioListener.pause = false;
    }

    public void FadeInToRestart()
    {
        m_fadeObj.GetComponent<Animator>().SetTrigger("Start");
        Invoker.InvokeDelayed(Restart, 2);
    }

    public void FadeInToMenu()
    {
        m_fadeObj.GetComponent<Animator>().SetTrigger("Start");
        Invoker.InvokeDelayed(ToMenu, 2);
    }

    public void CountDownToUnpause()
    {
        m_countDownObj.GetComponent<Animator>().Play("CountdownResume");
        Invoker.InvokeDelayed(PanelResume, 2);
        Invoker.InvokeDelayed(Unpause, 4);
    }

    private void PanelResume()
    {
        m_pausePanel.GetComponent<Animator>().SetTrigger("Resume");
    }

    private void ToMenu()
    {
        Time.timeScale = 1;
        PreviewTrack.Instance.ReturnToDefault();
        SceneManager.LoadScene(1);
        AudioListener.pause = false;
    }
}