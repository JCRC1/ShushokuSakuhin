using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaneIndexChecker : MonoBehaviour
{
    public InputField laneIndex;

    public Button[] eventCreateButtons;

    private void Update()
    {
        for (int i = 0; i < eventCreateButtons.Length; i++)
        {
            if (laneIndex.text != "None")
            {
                eventCreateButtons[i].interactable = true;
            }
            else
            {
                eventCreateButtons[i].interactable = false;
            }
        }
    }
}
