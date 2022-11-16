using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackPositionDisplayToggle : MonoBehaviour
{
    public static bool canSeek = false;
    public void DisplayToggle(GameObject _display)
    {
        _display.SetActive(!_display.activeSelf);
    }
    public void InteractToggle(InputField _input)
    {
        _input.interactable = !_input.interactable;
    }
    public void CanSeekToggle()
    {
        canSeek = !canSeek;
    }
}
