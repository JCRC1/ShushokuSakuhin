using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class DummyLaneMovement : MonoBehaviour
{
    public GameObject m_movementGizmoPrefab;
    public GameObject m_XmovementGizmoPrefab;
    public GameObject m_rotationGizmoPrefab;

    [HideInInspector]
    public GameObject m_movementGizmo;
    [HideInInspector]
    public GameObject m_XmovementGizmo;
    [HideInInspector]
    public GameObject m_rotationGizmo;

    private SpriteShapeController m_spriteShapeController;

    private void Awake()
    {
        m_spriteShapeController = GetComponent<SpriteShapeController>();
        
        m_movementGizmo = Instantiate(m_movementGizmoPrefab);
        m_movementGizmo.SetActive(false);

        m_XmovementGizmo = Instantiate(m_XmovementGizmoPrefab);
        m_XmovementGizmo.SetActive(false);

        m_rotationGizmo = Instantiate(m_rotationGizmoPrefab);
        m_rotationGizmo.SetActive(false);
    }
    public void StartUseM()
    {
        m_movementGizmo.SetActive(true);
        m_movementGizmo.GetComponent<MovementGizmo>().m_operating = true;
        m_movementGizmo.GetComponent<MovementGizmo>().SetTargetObject(gameObject);
    }

    public void EndUseM()
    {
        m_movementGizmo.GetComponent<MovementGizmo>().m_operating = false;
        m_movementGizmo.SetActive(false);
    }

    public void StartUseR()
    {
        m_rotationGizmo.SetActive(true);
        m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_operating = true;
        m_rotationGizmo.transform.GetChild(0).GetComponent< RotationGizmo>().SetTargetObject(gameObject);
    }

    public void EndUseR()
    {
        m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_operating = false;
        m_rotationGizmo.SetActive(false);
    }

    public void StartUseL()
    {
        m_XmovementGizmo.SetActive(true);
        m_XmovementGizmo.transform.rotation = transform.rotation;
        m_XmovementGizmo.GetComponent<MovementGizmo>().m_operating = true;
        m_XmovementGizmo.GetComponent<MovementGizmo>().SetTargetObject(transform.GetChild(0).gameObject);
    }

    public void EndUseL()
    {
        m_XmovementGizmo.GetComponent<MovementGizmo>().m_operating = false;
        m_XmovementGizmo.SetActive(false);
    }

    private void Update()
    {
        if (!ManualButton.m_manualBeatInput)
        {
            SelectedLaneDisplay.Instance.m_moveToXDisplay.text = transform.position.x.ToString("0.00");
            SelectedLaneDisplay.Instance.m_moveToYDisplay.text = transform.position.y.ToString("0.00");
            SelectedLaneDisplay.Instance.m_targetLengthDisplay.text = transform.GetChild(0).localPosition.x.ToString("0.00");

            SelectedLaneDisplay.Instance.m_angleDisplay.text = m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_currentAngle.ToString("0.00");

            if (SelectedLaneDisplay.Instance.m_pivotToggle.isOn)
                m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_pivot = new Vector2(m_rotationGizmo.transform.GetChild(0).transform.position.x - 5.0f, m_rotationGizmo.transform.GetChild(0).transform.position.y);
            else
                m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_pivot = new Vector2(m_rotationGizmo.transform.GetChild(0).transform.position.x, m_rotationGizmo.transform.GetChild(0).transform.position.y);
        }

        SpriteShapeUpdate();
    }

    private void SpriteShapeUpdate()
    {
        Spline spline = m_spriteShapeController.spline;
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

        m_spriteShapeController.RefreshSpriteShape();
    }
}
