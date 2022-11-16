using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateLaneEvent: MonoBehaviour
{
    public static bool creatingMovement = false;
    public static bool creatingRotation = false;
    public static bool creatingFade = false;
    public static bool creatingLength = false;

    public Camera worldCam;
    public GameObject dummyLanePrefab;
    private GameObject dummyLane;
    private float timeOfStart;

    public LaneEventMovement   tempMovement;
    public LaneEventRotation   tempRotation;
    public LaneEventFade       tempFade;
    public LaneEventLength     tempLength;

    private void Start()
    {
        dummyLane = Instantiate(dummyLanePrefab);
        dummyLane.SetActive(false);
    }

    public void CreateMovementButton()
    {
        creatingMovement = true;

        dummyLane.transform.position = LaneEditor.Instance.selectedLane.transform.position;
        dummyLane.transform.rotation = LaneEditor.Instance.selectedLane.transform.rotation;

        dummyLane.SetActive(true);
        dummyLane.GetComponent<DummyLaneMovement>().StartUseM();
        timeOfStart = LevelEditorManager.Instance.trackPosInBeats;
    }

    public void EndMovementButton()
    {
        dummyLane.GetComponent<DummyLaneMovement>().EndUseM();

        tempMovement = new LaneEventMovement();

        float.TryParse(Seekbar.Instance.currentBeatText[0].text, out tempMovement.beat);
        float.TryParse(SelectedLaneDisplay.Instance.movementDurationDisplay.text, out tempMovement.duration);
        float.TryParse(SelectedLaneDisplay.Instance.moveToXDisplay.text, out tempMovement.targetPosition.x);
        float.TryParse(SelectedLaneDisplay.Instance.moveToYDisplay.text, out tempMovement.targetPosition.y);
        tempMovement.easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.movementEaseSelect.value;

        LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventsMovement.Add(tempMovement);
        LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventsMovement = LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventsMovement.OrderBy(lst => lst.beat).ToList();
        creatingMovement = false;
        dummyLane.SetActive(false);

        EventListDisplay.Instance.AddEventToList(tempMovement, LaneEditor.Instance.selectedLane.identifier);
    }

    public void CancelMovementButton()
    {
        creatingLength = false;
        dummyLane.GetComponent<DummyLaneMovement>().EndUseM();
        dummyLane.SetActive(false);
    }

    public void CreateRotationButton()
    {
        creatingRotation = true;

        dummyLane.transform.position = LaneEditor.Instance.selectedLane.transform.position;
        dummyLane.transform.rotation = LaneEditor.Instance.selectedLane.transform.rotation;

        dummyLane.SetActive(true);
        dummyLane.GetComponent<DummyLaneMovement>().StartUseR();
        timeOfStart = LevelEditorManager.Instance.trackPosInBeats;
    }

    public void EndRotationButton()
    {
        dummyLane.GetComponent<DummyLaneMovement>().EndUseR();

        tempRotation = new LaneEventRotation();

        float.TryParse(Seekbar.Instance.currentBeatText[0].text, out tempRotation.beat);
        float.TryParse(SelectedLaneDisplay.Instance.rotationDurationDisplay.text, out tempRotation.duration);
        float.TryParse(SelectedLaneDisplay.Instance.angleDisplay.text, out tempRotation.targetRotation);
        tempRotation.pivotAtEnd = SelectedLaneDisplay.Instance.pivotToggle.isOn;
        tempRotation.easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.rotationEaseSelect.value;

        LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventsRotation.Add(tempRotation);
        LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventsRotation = LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventsRotation.OrderBy(lst => lst.beat).ToList();

        creatingRotation = false;
        dummyLane.SetActive(false);

        EventListDisplay.Instance.AddEventToList(tempRotation, LaneEditor.Instance.selectedLane.identifier);
    }
    public void CancelRotationButton()
    {
        creatingLength = false;
        dummyLane.GetComponent<DummyLaneMovement>().EndUseR();
        dummyLane.SetActive(false);
    }

    public void CreateFadeButton()
    {
        creatingFade = true;

        timeOfStart = LevelEditorManager.Instance.trackPosInBeats;
    }

    public void EndFadeButton()
    {
        tempFade = new LaneEventFade();

        float.TryParse(Seekbar.Instance.currentBeatText[0].text, out tempFade.beat);
        float.TryParse(SelectedLaneDisplay.Instance.fadeDurationDisplay.text, out tempFade.duration);
        float.TryParse(SelectedLaneDisplay.Instance.targetAlphaDisplay.text, out tempFade.targetAlpha);
        tempFade.fadeNotes = SelectedLaneDisplay.Instance.fadeNotesToggle.isOn;
        tempFade.easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.fadeEaseSelect.value;

        LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventFade.Add(tempFade);
        LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventFade = LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventFade.OrderBy(lst => lst.beat).ToList();

        creatingFade = false;

        EventListDisplay.Instance.AddEventToList(tempFade, LaneEditor.Instance.selectedLane.identifier);
    }
    public void CancelFadeButton()
    {
        creatingLength = false;
    }


    public void CreateLengthButton()
    {
        creatingLength = true;

        dummyLane.transform.position = LaneEditor.Instance.selectedLane.transform.position;
        dummyLane.transform.GetChild(0).localPosition = LaneEditor.Instance.selectedLane.transform.GetChild(0).localPosition;
        dummyLane.transform.GetChild(0).rotation = LaneEditor.Instance.selectedLane.transform.GetChild(0).rotation;
        dummyLane.transform.rotation = LaneEditor.Instance.selectedLane.transform.rotation;

        dummyLane.SetActive(true);
        dummyLane.GetComponent<DummyLaneMovement>().StartUseL();
        timeOfStart = LevelEditorManager.Instance.trackPosInBeats;
    }

    public void EndLengthButton()
    {
        dummyLane.GetComponent<DummyLaneMovement>().EndUseL();

        tempLength = new LaneEventLength();

        float.TryParse(Seekbar.Instance.currentBeatText[0].text, out tempLength.beat);
        float.TryParse(SelectedLaneDisplay.Instance.lengthDurationDisplay.text, out tempLength.duration);
        float.TryParse(SelectedLaneDisplay.Instance.targetLengthDisplay.text, out tempLength.targetLength);
        tempLength.easeType = (LaneEvent.EaseType)SelectedLaneDisplay.Instance.lengthEaseSelect.value;

        LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventLength.Add(tempLength);
        LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventLength = LevelEditorManager.Instance.chartData.lane[LaneEditor.Instance.selectedLane.identifier].laneEventLength.OrderBy(lst => lst.beat).ToList();

        creatingLength = false;
        dummyLane.SetActive(false);

        EventListDisplay.Instance.AddEventToList(tempLength, LaneEditor.Instance.selectedLane.identifier);
    }

    public void CancelLengthButton()
    {
        creatingLength = false;
        dummyLane.GetComponent<DummyLaneMovement>().EndUseL();
        dummyLane.SetActive(false);
    }

    public void SetPositionX(Text _text)
    {
        dummyLane.transform.position = new Vector2(float.Parse(_text.text), dummyLane.transform.position.y);
    }

    public void SetPositionY(Text _text)
    {
        dummyLane.transform.position = new Vector2(dummyLane.transform.position.x, float.Parse(_text.text));
    }

    public void SetRotation(Text _text)
    {
        dummyLane.transform.rotation = Quaternion.Euler(0.0f, 0.0f, float.Parse(_text.text));
    }

    public void SetPivotX(Text _text)
    {
        dummyLane.GetComponent<DummyLaneMovement>().rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().pivot = new Vector2(float.Parse(_text.text), dummyLane.GetComponent<DummyLaneMovement>().rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().pivot.y);
    }

    public void SetPivotY(Text _text)
    {
        dummyLane.GetComponent<DummyLaneMovement>().rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().pivot = new Vector2(dummyLane.GetComponent<DummyLaneMovement>().rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().pivot.x, float.Parse(_text.text));
    }
    public void SetLength(Text _text)
    {
        dummyLane.transform.GetChild(0).localPosition = new Vector2(float.Parse(_text.text), 0);
    }

    private void Update()
    {
        if (!ManualButton.manualBeatInput)
        {
            if (creatingMovement)
            {
                SelectedLaneDisplay.Instance.movementDurationDisplay.text = (LevelEditorManager.Instance.trackPosInBeats - timeOfStart).ToString("0.00");
            }

            if (creatingRotation)
            {
                SelectedLaneDisplay.Instance.rotationDurationDisplay.text = (LevelEditorManager.Instance.trackPosInBeats - timeOfStart).ToString("0.00");
            }

            if (creatingLength)
            {
                SelectedLaneDisplay.Instance.lengthDurationDisplay.text = (LevelEditorManager.Instance.trackPosInBeats - timeOfStart).ToString("0.00");
            }
        }
    }
}
