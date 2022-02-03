using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedLaneDisplay : MonoBehaviour
{
    public static SelectedLaneDisplay Instance;

    // Contains reference to the display boxes
    public InputField[] m_indexDisplay;

    public InputField m_moveToXDisplay;
    public InputField m_moveToYDisplay;
    public InputField m_movementDurationDisplay;
    public Dropdown m_movementEaseSelect;

    public InputField m_angleDisplay;
    public InputField m_rotationDurationDisplay;
    public Toggle m_pivotToggle;
    public Dropdown m_rotationEaseSelect;

    public InputField m_targetAlphaDisplay;
    public InputField m_fadeDurationDisplay;
    public Toggle m_fadeNotesToggle;
    public Dropdown m_fadeEaseSelect;

    public InputField m_targetLengthDisplay;
    public InputField m_lengthDurationDisplay;
    public Dropdown m_lengthEaseSelect;

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
                for (int i = 0; i < m_indexDisplay.Length; i++)
                {
                    m_indexDisplay[i].text = LaneEditor.Instance.m_selectedLane.m_identifier.ToString();
                }
            }
            else
            {
                for (int i = 0; i < m_indexDisplay.Length; i++)
                {
                    m_indexDisplay[i].text = "None";
                }
            }
        }
    }
}
