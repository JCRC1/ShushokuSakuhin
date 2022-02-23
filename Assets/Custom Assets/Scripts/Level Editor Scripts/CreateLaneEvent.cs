using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateLaneEvent: MonoBehaviour
{
    public static bool m_creatingMovement = false;
    public static bool m_creatingRotation = false;
    public static bool m_creatingFade = false;
    public static bool m_creatingLength = false;

    public Camera m_worldCam;
    public GameObject m_dummyLanePrefab;
    private GameObject m_dummyLane;
    private float m_timeOfStart;

    public LaneEventMovement   m_tempMovement;
    public LaneEventRotation   m_tempRotation;
    public LaneEventFade       m_tempFade;
    public LaneEventLength     m_tempLength;

    private void Start()
    {
        m_dummyLane = Instantiate(m_dummyLanePrefab);
        m_dummyLane.SetActive(false);
    }

    public void CreateMovementButton()
    {
        m_creatingMovement = true;

        m_dummyLane.transform.position = LaneEditor.Instance.m_selectedLane.transform.position;
        m_dummyLane.transform.rotation = LaneEditor.Instance.m_selectedLane.transform.rotation;

        m_dummyLane.SetActive(true);
        m_dummyLane.GetComponent<DummyLaneMovement>().StartUseM();
        m_timeOfStart = LevelEditorManager.Instance.m_trackPosInBeats;
    }

    public void EndMovementButton()
    {
        m_dummyLane.GetComponent<DummyLaneMovement>().EndUseM();

        m_tempMovement = new LaneEventMovement();

        float.TryParse(Seekbar.Instance.m_currentBeatText[0].text, out m_tempMovement.m_beat);
        float.TryParse(SelectedLaneDisplay.Instance.m_movementDurationDisplay.text, out m_tempMovement.m_duration);
        float.TryParse(SelectedLaneDisplay.Instance.m_moveToXDisplay.text, out m_tempMovement.m_targetPosition.x);
        float.TryParse(SelectedLaneDisplay.Instance.m_moveToYDisplay.text, out m_tempMovement.m_targetPosition.y);
        m_tempMovement.m_easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.m_movementEaseSelect.value;

        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventsMovement.Add(m_tempMovement);
        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventsMovement = LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventsMovement.OrderBy(lst => lst.m_beat).ToList();
        m_creatingMovement = false;
        m_dummyLane.SetActive(false);

        EventListDisplay.Instance.AddEventToList(m_tempMovement, LaneEditor.Instance.m_selectedLane.m_identifier);
    }

    public void CancelMovementButton()
    {
        m_creatingLength = false;
        m_dummyLane.GetComponent<DummyLaneMovement>().EndUseM();
        m_dummyLane.SetActive(false);
    }

    public void CreateRotationButton()
    {
        m_creatingRotation = true;

        m_dummyLane.transform.position = LaneEditor.Instance.m_selectedLane.transform.position;
        m_dummyLane.transform.rotation = LaneEditor.Instance.m_selectedLane.transform.rotation;

        m_dummyLane.SetActive(true);
        m_dummyLane.GetComponent<DummyLaneMovement>().StartUseR();
        m_timeOfStart = LevelEditorManager.Instance.m_trackPosInBeats;
    }

    public void EndRotationButton()
    {
        m_dummyLane.GetComponent<DummyLaneMovement>().EndUseR();

        m_tempRotation = new LaneEventRotation();

        float.TryParse(Seekbar.Instance.m_currentBeatText[0].text, out m_tempRotation.m_beat);
        float.TryParse(SelectedLaneDisplay.Instance.m_rotationDurationDisplay.text, out m_tempRotation.m_duration);
        float.TryParse(SelectedLaneDisplay.Instance.m_angleDisplay.text, out m_tempRotation.m_targetRotation);
        m_tempRotation.m_pivotAtEnd = SelectedLaneDisplay.Instance.m_pivotToggle.isOn;
        m_tempRotation.m_easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.m_rotationEaseSelect.value;

        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventsRotation.Add(m_tempRotation);
        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventsRotation = LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventsRotation.OrderBy(lst => lst.m_beat).ToList();

        m_creatingRotation = false;
        m_dummyLane.SetActive(false);

        EventListDisplay.Instance.AddEventToList(m_tempRotation, LaneEditor.Instance.m_selectedLane.m_identifier);
    }
    public void CancelRotationButton()
    {
        m_creatingLength = false;
        m_dummyLane.GetComponent<DummyLaneMovement>().EndUseR();
        m_dummyLane.SetActive(false);
    }

    public void CreateFadeButton()
    {
        m_creatingFade = true;

        m_timeOfStart = LevelEditorManager.Instance.m_trackPosInBeats;
    }

    public void EndFadeButton()
    {
        m_tempFade = new LaneEventFade();

        float.TryParse(Seekbar.Instance.m_currentBeatText[0].text, out m_tempFade.m_beat);
        float.TryParse(SelectedLaneDisplay.Instance.m_fadeDurationDisplay.text, out m_tempFade.m_duration);
        float.TryParse(SelectedLaneDisplay.Instance.m_targetAlphaDisplay.text, out m_tempFade.m_targetAlpha);
        m_tempFade.m_fadeNotes = SelectedLaneDisplay.Instance.m_fadeNotesToggle.isOn;
        m_tempFade.m_easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.m_fadeEaseSelect.value;

        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventFade.Add(m_tempFade);
        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventFade = LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventFade.OrderBy(lst => lst.m_beat).ToList();

        m_creatingFade = false;

        EventListDisplay.Instance.AddEventToList(m_tempFade, LaneEditor.Instance.m_selectedLane.m_identifier);
    }
    public void CancelFadeButton()
    {
        m_creatingLength = false;
    }


    public void CreateLengthButton()
    {
        m_creatingLength = true;

        m_dummyLane.transform.position = LaneEditor.Instance.m_selectedLane.transform.position;
        m_dummyLane.transform.GetChild(0).localPosition = LaneEditor.Instance.m_selectedLane.transform.GetChild(0).localPosition;
        m_dummyLane.transform.GetChild(0).rotation = LaneEditor.Instance.m_selectedLane.transform.GetChild(0).rotation;
        m_dummyLane.transform.rotation = LaneEditor.Instance.m_selectedLane.transform.rotation;

        m_dummyLane.SetActive(true);
        m_dummyLane.GetComponent<DummyLaneMovement>().StartUseL();
        m_timeOfStart = LevelEditorManager.Instance.m_trackPosInBeats;
    }

    public void EndLengthButton()
    {
        m_dummyLane.GetComponent<DummyLaneMovement>().EndUseL();

        m_tempLength = new LaneEventLength();

        float.TryParse(Seekbar.Instance.m_currentBeatText[0].text, out m_tempLength.m_beat);
        float.TryParse(SelectedLaneDisplay.Instance.m_lengthDurationDisplay.text, out m_tempLength.m_duration);
        float.TryParse(SelectedLaneDisplay.Instance.m_targetLengthDisplay.text, out m_tempLength.m_targetLength);
        m_tempLength.m_easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.m_lengthEaseSelect.value;

        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventLength.Add(m_tempLength);
        LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventLength = LevelEditorManager.Instance.m_chartData.m_lane[LaneEditor.Instance.m_selectedLane.m_identifier].m_laneEventLength.OrderBy(lst => lst.m_beat).ToList();

        m_creatingLength = false;
        m_dummyLane.SetActive(false);

        EventListDisplay.Instance.AddEventToList(m_tempLength, LaneEditor.Instance.m_selectedLane.m_identifier);
    }

    public void CancelLengthButton()
    {
        m_creatingLength = false;
        m_dummyLane.GetComponent<DummyLaneMovement>().EndUseL();
        m_dummyLane.SetActive(false);
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
    public void SetLength(Text _text)
    {
        m_dummyLane.transform.GetChild(0).localPosition = new Vector2(float.Parse(_text.text), 0);
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

            if (m_creatingLength)
            {
                SelectedLaneDisplay.Instance.m_lengthDurationDisplay.text = (LevelEditorManager.Instance.m_trackPosInBeats - m_timeOfStart).ToString("0.00");
            }
        }
    }
}
