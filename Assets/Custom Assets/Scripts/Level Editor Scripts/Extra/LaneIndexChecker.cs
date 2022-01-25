using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaneIndexChecker : MonoBehaviour
{
    public InputField m_laneIndex;

    public Button[] m_eventCreateButtons;

    private void Update()
    {
        for (int i = 0; i < m_eventCreateButtons.Length; i++)
        {
            if (m_laneIndex.text != "None")
            {
                m_eventCreateButtons[i].interactable = true;
            }
            else
            {
                m_eventCreateButtons[i].interactable = false;
            }
        }
    }
}
