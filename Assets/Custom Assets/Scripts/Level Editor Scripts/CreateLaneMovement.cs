using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLaneMovement : MonoBehaviour
{
    public static bool m_creatingMovement = false;
    public static bool m_creatingRotation = false;

    public Camera m_worldCam;
    public GameObject m_dummyLanePrefab;
    private GameObject m_dummyLane;
    private float m_timeOfStart;

    private void Start()
    {
        m_dummyLane = Instantiate(m_dummyLanePrefab);
        m_dummyLane.SetActive(false);
    }

    public void CreateMovementButton()
    {
        m_creatingMovement = true;
        m_dummyLane.SetActive(true);
        m_dummyLane.GetComponent<DummyLaneMovement>().StartUseM();

        m_dummyLane.transform.position = LaneEditor.Instance.m_selectedLane.transform.position;
        m_dummyLane.transform.rotation = LaneEditor.Instance.m_selectedLane.transform.rotation;

        m_timeOfStart = LevelEditorManager.Instance.m_trackPosInBeats;
    }

    public void EndMovementButton()
    {
        m_dummyLane.GetComponent<DummyLaneMovement>().EndUseM();

        LaneEventMovement newMovement = new LaneEventMovement();

        float.TryParse(Seekbar.Instance.m_currentBeatText.text, out newMovement.m_beat);
        float.TryParse(SelectedLaneDisplay.Instance.m_movementDurationDisplay.text, out newMovement.m_duration);
        float.TryParse(SelectedLaneDisplay.Instance.m_moveToXDisplay.text, out newMovement.m_targetPosition.x);
        float.TryParse(SelectedLaneDisplay.Instance.m_moveToYDisplay.text, out newMovement.m_targetPosition.y);
        newMovement.m_easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.m_movementEaseSelect.value;

        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventsMovement.Add(newMovement);
        m_creatingMovement = false;
        m_dummyLane.SetActive(false);
    }

    public void CancelMovementButton()
    {
        int selected = LaneEditor.Instance.m_selectedLane.m_identifier;
        EndMovementButton(); 
        LevelEditorManager.Instance.m_chartData.m_lane[selected].m_laneEventsMovement.Remove(LevelEditorManager.Instance.m_chartData.m_lane[selected].m_laneEventsMovement[LevelEditorManager.Instance.m_chartData.m_lane[selected].m_laneEventsMovement.Count - 1]);
    }

    public void CreateRotationButton()
    {
        m_creatingRotation = true;
        m_dummyLane.SetActive(true);

        m_dummyLane.GetComponent<DummyLaneMovement>().StartUseR();

        m_dummyLane.transform.position = LaneEditor.Instance.m_selectedLane.transform.position;
        m_dummyLane.transform.rotation = LaneEditor.Instance.m_selectedLane.transform.rotation;
        
        m_timeOfStart = LevelEditorManager.Instance.m_trackPosInBeats;
    }

    public void EndRotationButton()
    {
        m_dummyLane.GetComponent<DummyLaneMovement>().EndUseR();

        LaneEventRotation newRotation = new LaneEventRotation();

        float.TryParse(Seekbar.Instance.m_currentBeatText.text, out newRotation.m_beat);
        float.TryParse(SelectedLaneDisplay.Instance.m_rotationDurationDisplay.text, out newRotation.m_duration);
        float.TryParse(SelectedLaneDisplay.Instance.m_angleDisplay.text, out newRotation.m_targetRotation);
        newRotation.m_pivotAtEnd = SelectedLaneDisplay.Instance.m_pivotToggle.isOn;
        newRotation.m_easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.m_rotationEaseSelect.value;

        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventsRotation.Add(newRotation);
        m_creatingRotation = false;
        m_dummyLane.SetActive(false);
    }
    public void CancelRotationButton()
    {
        int selected = LaneEditor.Instance.m_selectedLane.m_identifier;
        EndRotationButton();
        LevelEditorManager.Instance.m_chartData.m_lane[selected].m_laneEventsRotation.Remove(LevelEditorManager.Instance.m_chartData.m_lane[selected].m_laneEventsRotation[LevelEditorManager.Instance.m_chartData.m_lane[selected].m_laneEventsRotation.Count - 1]);
    }

    public void SetPositionX(Text _text)
    {
        m_dummyLane.transform.position = new Vector2(float.Parse(_text.text), m_dummyLane.transform.position.y);
    }

    public void SetPositionY(Text _text)
    {
        m_dummyLane.transform.position = new Vector2(m_dummyLane.transform.position.x, float.Parse(_text.text));
    }

    public void SetRotation(Text _text)
    {
        m_dummyLane.transform.rotation = Quaternion.Euler(0.0f, 0.0f, float.Parse(_text.text));
    }

    public void SetPivotX(Text _text)
    {
        m_dummyLane.GetComponent<DummyLaneMovement>().m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_pivot = new Vector2(float.Parse(_text.text), m_dummyLane.GetComponent<DummyLaneMovement>().m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_pivot.y);
    }

    public void SetPivotY(Text _text)
    {
        m_dummyLane.GetComponent<DummyLaneMovement>().m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_pivot = new Vector2(m_dummyLane.GetComponent<DummyLaneMovement>().m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_pivot.x, float.Parse(_text.text));
    }

    private void Update()
    {
        if (!ManualButton.m_manualBeatInput)
        {
            if (m_creatingMovement)
            {
                SelectedLaneDisplay.Instance.m_movementDurationDisplay.text = (LevelEditorManager.Instance.m_trackPosInBeats - m_timeOfStart).ToString("0.00");
            }

            if (m_creatingRotation)
            {
                SelectedLaneDisplay.Instance.m_rotationDurationDisplay.text = (LevelEditorManager.Instance.m_trackPosInBeats - m_timeOfStart).ToString("0.00");
            }
        }
    }
}
