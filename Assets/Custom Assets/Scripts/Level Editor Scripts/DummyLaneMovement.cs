using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class DummyLaneMovement : MonoBehaviour
{
    public GameObject movementGizmoPrefab;
    public GameObject XmovementGizmoPrefab;
    public GameObject rotationGizmoPrefab;

    [HideInInspector]
    public GameObject movementGizmo;
    [HideInInspector]
    public GameObject XmovementGizmo;
    [HideInInspector]
    public GameObject rotationGizmo;

    private SpriteShapeController spriteShapeController;

    private void Awake()
    {
        spriteShapeController = GetComponent<SpriteShapeController>();
        
        movementGizmo = Instantiate(movementGizmoPrefab);
        movementGizmo.SetActive(false);

        XmovementGizmo = Instantiate(XmovementGizmoPrefab);
        XmovementGizmo.SetActive(false);

        rotationGizmo = Instantiate(rotationGizmoPrefab);
        rotationGizmo.SetActive(false);
    }
    public void StartUseM()
    {
        movementGizmo.SetActive(true);
        movementGizmo.GetComponent<MovementGizmo>().operating = true;
        movementGizmo.GetComponent<MovementGizmo>().SetTargetObject(gameObject);
    }

    public void EndUseM()
    {
        movementGizmo.GetComponent<MovementGizmo>().operating = false;
        movementGizmo.SetActive(false);
    }

    public void StartUseR()
    {
        rotationGizmo.SetActive(true);
        rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().operating = true;
        rotationGizmo.transform.GetChild(0).GetComponent< RotationGizmo>().SetTargetObject(gameObject);
    }

    public void EndUseR()
    {
        rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().operating = false;
        rotationGizmo.SetActive(false);
    }

    public void StartUseL()
    {
        XmovementGizmo.SetActive(true);
        XmovementGizmo.transform.rotation = transform.rotation;
        XmovementGizmo.GetComponent<MovementGizmo>().operating = true;
        XmovementGizmo.GetComponent<MovementGizmo>().SetTargetObject(transform.GetChild(0).gameObject);
    }

    public void EndUseL()
    {
        XmovementGizmo.GetComponent<MovementGizmo>().operating = false;
        XmovementGizmo.SetActive(false);
    }

    private void Update()
    {
        if (!ManualButton.manualBeatInput)
        {
            SelectedLaneDisplay.Instance.moveToXDisplay.text = transform.position.x.ToString("0.00");
            SelectedLaneDisplay.Instance.moveToYDisplay.text = transform.position.y.ToString("0.00");
            SelectedLaneDisplay.Instance.targetLengthDisplay.text = transform.GetChild(0).localPosition.x.ToString("0.00");

            SelectedLaneDisplay.Instance.angleDisplay.text = rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().currentAngle.ToString("0.00");

            if (SelectedLaneDisplay.Instance.pivotToggle.isOn)
                rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().pivot = new Vector2(rotationGizmo.transform.GetChild(0).transform.position.x - 5.0f, rotationGizmo.transform.GetChild(0).transform.position.y);
            else
                rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().pivot = new Vector2(rotationGizmo.transform.GetChild(0).transform.position.x, rotationGizmo.transform.GetChild(0).transform.position.y);
        }

        SpriteShapeUpdate();
    }

    private void SpriteShapeUpdate()
    {
        Spline spline = spriteShapeController.spline;
        spline.Clear();

        // Lock Y Movement
        transform.GetChild(0).localPosition = new Vector2(transform.GetChild(0).localPosition.x, 0);
        transform.GetChild(1).localPosition = new Vector2(transform.GetChild(1).localPosition.x, 0);

        GameObject start = transform.GetChild(0).gameObject;
        GameObject end = transform.GetChild(1).gameObject;

        if (start.transform.localPosition.x >= 0)
        {
            spline.InsertPointAt(0, new Vector2(start.transform.localPosition.x + 0.5f, start.transform.localPosition.y + 0.5f));
            spline.InsertPointAt(0, new Vector2(start.transform.localPosition.x + 0.5f, start.transform.localPosition.y - 0.5f));
        }
        else
        {
            spline.InsertPointAt(0, new Vector2(start.transform.localPosition.x - 0.5f, start.transform.localPosition.y + 0.5f));
            spline.InsertPointAt(0, new Vector2(start.transform.localPosition.x - 0.5f, start.transform.localPosition.y - 0.5f));
        }

        spline.InsertPointAt(0, new Vector2(end.transform.localPosition.x - 0.5f, end.transform.localPosition.y - 0.5f));
        spline.InsertPointAt(0, new Vector2(end.transform.localPosition.x - 0.5f, end.transform.localPosition.y + 0.5f));

        spriteShapeController.RefreshSpriteShape();
    }
}
