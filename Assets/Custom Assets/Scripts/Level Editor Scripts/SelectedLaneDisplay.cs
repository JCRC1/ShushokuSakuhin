using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedLaneDisplay : MonoBehaviour
{
    public static SelectedLaneDisplay Instance;

    // Contains reference to the display boxes
    public InputField[] indexDisplay;

    public InputField moveToXDisplay;
    public InputField moveToYDisplay;
    public InputField movementDurationDisplay;
    public Dropdown movementEaseSelect;

    public InputField angleDisplay;
    public InputField rotationDurationDisplay;
    public Toggle pivotToggle;
    public Dropdown rotationEaseSelect;

    public InputField targetAlphaDisplay;
    public InputField fadeDurationDisplay;
    public Toggle fadeNotesToggle;
    public Dropdown fadeEaseSelect;

    public InputField targetLengthDisplay;
    public InputField lengthDurationDisplay;
    public Dropdown lengthEaseSelect;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ManualButton.manualBeatInput)
        {
            if (LaneEditor.Instance.selectedLane)
            {
                for (int i = 0; i < indexDisplay.Length; i++)
                {
                    indexDisplay[i].text = LaneEditor.Instance.selectedLane.identifier.ToString();
                }
            }
            else
            {
                for (int i = 0; i < indexDisplay.Length; i++)
                {
                    indexDisplay[i].text = "None";
                }
            }
        }
    }
}
