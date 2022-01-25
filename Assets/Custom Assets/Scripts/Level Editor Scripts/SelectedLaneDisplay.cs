using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedLaneDisplay : MonoBehaviour
{
    public static SelectedLaneDisplay Instance;

    // Contains reference to the display boxes
    public InputField m_indexDisplay;

    public InputField m_moveToXDisplay;
    public InputField m_moveToYDisplay;
    public InputField m_movementDurationDisplay;
    public Dropdown m_movementEaseSelect;

    public InputField m_angleDisplay;
    public InputField m_rotationDurationDisplay;
    public InputField m_pivotXDisplay;
    public InputField m_pivotYDisplay;
    public Dropdown m_rotationEaseSelect;


    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ManualButton.m_manualBeatInput)
        {
            if (LaneEditor.Instance.m_selectedLane)
            {
                m_indexDisplay.text = LaneEditor.Instance.m_selectedLane.m_identifier.ToString();
            }
            else
            {
                m_indexDisplay.text = "None";
            }
        }
    }
}
