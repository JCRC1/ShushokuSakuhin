using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualButton : MonoBehaviour
{
    public static bool manualBeatInput = false;

    public void ManualInput()
    {
        manualBeatInput = !manualBeatInput;
    }

    public void InputOnOff(InputField _inputField)
    {
        _inputField.interactable = manualBeatInput;
    }
}
