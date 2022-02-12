using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMFaderChecker : MonoBehaviour
{
    public void SwapMusic(AudioClip _newClip)
    {
        if (PreviewTrack.Instance)
        {
            PreviewTrack.Instance.SwapTrack(_newClip);
        }
    }
}
