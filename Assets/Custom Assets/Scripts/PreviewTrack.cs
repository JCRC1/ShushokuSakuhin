using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewTrack : MonoBehaviour
{
    public AudioClip m_defaultBGM;
    public float m_crossfadeTime;

    public bool m_isPlayingTrack01;

    private AudioSource m_track01;
    private AudioSource m_track02;

    private static PreviewTrack _instance;
    public static PreviewTrack Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_track01 = gameObject.AddComponent<AudioSource>();
        m_track02 = gameObject.AddComponent<AudioSource>();

        m_isPlayingTrack01 = true;

        ReturnToDefault();
    }

    private void Update()
    {
        if (GameManager.Instance)
        {
            if (!GameManager.Instance.m_finalized)
            {
                m_track01.Stop();
                m_track02.Stop();
            }
            else if (m_isPlayingTrack01)
            {
                if (!m_track01.isPlaying)
                    ReturnToDefault();
            } 
            else if (!m_isPlayingTrack01)
            {
                if (!m_track02.isPlaying)
                    ReturnToDefault();
            }
        }
    }

    public void SwapTrack(AudioClip _newClip)
    {
        StopAllCoroutines();
        CancelInvoke();

        StartCoroutine(CrossFade(_newClip));

        if (_newClip != m_defaultBGM)
        {
            Invoke("ReturnToDefault", _newClip.length - m_crossfadeTime);
        }

        m_isPlayingTrack01 = !m_isPlayingTrack01;
    }

    public void ReturnToDefault()
    {
        SwapTrack(m_defaultBGM);     
    }

    IEnumerator CrossFade(AudioClip _newClip)
    {
        float timeElapsed = 0.0f;

        if (m_isPlayingTrack01)
        {
            if(_newClip == m_defaultBGM)
                m_track02.loop = true;
            else
                m_track02.loop = false;

            m_track02.clip = _newClip;
            m_track02.Play();

            while (timeElapsed < m_crossfadeTime)
            {
                m_track02.volume = Mathf.Lerp(0, 0.05f, timeElapsed / m_crossfadeTime);
                m_track01.volume = Mathf.Lerp(0.05f, 0, timeElapsed / m_crossfadeTime);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
            m_track01.Stop();
        }
        else
        {
            if (_newClip == m_defaultBGM)
                m_track01.loop = true;
            else
                m_track01.loop = false;

            m_track01.clip = _newClip;
            m_track01.Play();
            while (timeElapsed < m_crossfadeTime)
            {
                m_track01.volume = Mathf.Lerp(0, 0.05f, timeElapsed / m_crossfadeTime);
                m_track02.volume = Mathf.Lerp(0.05f, 0, timeElapsed / m_crossfadeTime);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
            m_track02.Stop();
        }
    }
}
