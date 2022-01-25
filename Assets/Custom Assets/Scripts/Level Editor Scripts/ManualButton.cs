using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualButton : MonoBehaviour
{
    public static bool m_manualBeatInput = false;

    public void ManualInput()
    {
        m_manualBeatInput = !m_manualBeatInput;
    }

    public void InputOnOff(InputField _inputField)
    {
        _inputField.interactable = !_inputField.interactable;
    }
}
