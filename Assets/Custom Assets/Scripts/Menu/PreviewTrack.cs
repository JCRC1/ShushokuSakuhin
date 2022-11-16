using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PreviewTrack : MonoBehaviour
{
    public AudioClip defaultBGM;
    public float crossfadeTime;
    public AudioMixerGroup bgmMixerGroup;

    public bool isPlayingTrack01;

    private AudioSource track01;
    private AudioSource track02;

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
        track01 = gameObject.AddComponent<AudioSource>();
        track01.outputAudioMixerGroup = bgmMixerGroup;
        track02 = gameObject.AddComponent<AudioSource>();
        track02.outputAudioMixerGroup = bgmMixerGroup;

        isPlayingTrack01 = true;

        ReturnToDefault();
    }

    private void Update()
    {
        if (GameManager.Instance)
        {
            if (!GameManager.Instance.finalized)
            {
                track01.Stop();
                track02.Stop();
            }
            else if (isPlayingTrack01)
            {
                if (!track01.isPlaying)
                    ReturnToDefault();
            } 
            else if (!isPlayingTrack01)
            {
                if (!track02.isPlaying)
                    ReturnToDefault();
            }
        }
    }

    public void SwapTrack(AudioClip _newClip)
    {
        StopAllCoroutines();
        CancelInvoke();

        StartCoroutine(CrossFade(_newClip));

        if (_newClip != defaultBGM)
        {
            Invoke("ReturnToDefault", _newClip.length - crossfadeTime);
        }

        isPlayingTrack01 = !isPlayingTrack01;
    }

    public void ReturnToDefault()
    {
        SwapTrack(defaultBGM);     
    }

    IEnumerator CrossFade(AudioClip _newClip)
    {
        float timeElapsed = 0.0f;

        if (isPlayingTrack01)
        {
            if(_newClip == defaultBGM)
                track02.loop = true;
            else
                track02.loop = false;

            track02.clip = _newClip;
            track02.Play();

            while (timeElapsed < crossfadeTime)
            {
                track02.volume = Mathf.Lerp(0, 0.05f, timeElapsed / crossfadeTime);
                track01.volume = Mathf.Lerp(0.05f, 0, timeElapsed / crossfadeTime);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
            track01.Stop();
        }
        else
        {
            if (_newClip == defaultBGM)
                track01.loop = true;
            else
                track01.loop = false;

            track01.clip = _newClip;
            track01.Play();
            while (timeElapsed < crossfadeTime)
            {
                track01.volume = Mathf.Lerp(0, 0.05f, timeElapsed / crossfadeTime);
                track02.volume = Mathf.Lerp(0.05f, 0, timeElapsed / crossfadeTime);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
            track02.Stop();
        }
    }
}
