using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyLaneMovement : MonoBehaviour
{
    public GameObject m_movementGizmoPrefab;
    public GameObject m_rotationGizmoPrefab;

    [HideInInspector]
    public GameObject m_movementGizmo;
    [HideInInspector]
    public GameObject m_rotationGizmo;

    private void Awake()
    {
        m_movementGizmo = Instantiate(m_movementGizmoPrefab);
        m_movementGizmo.SetActive(false);

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

    public void StartUseR(float _pivotX, float _pivotY)
    {
        m_rotationGizmo.SetActive(true);
        m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_operating = true;
        m_rotationGizmo.transform.GetChild(0).GetComponent< RotationGizmo>().SetTargetObject(gameObject);
        m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_pivot = new Vector2(_pivotX, _pivotY);
    }

    public void EndUseR()
    {
        m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_operating = false;
        m_rotationGizmo.SetActive(false);
    }

    private void Update()
    {
        if (!ManualButton.m_manualBeatInput)
        {
            SelectedLaneDisplay.Instance.m_moveToXDisplay.text = transform.position.x.ToString("0.00");
            SelectedLaneDisplay.Instance.m_moveToYDisplay.text = transform.position.y.ToString("0.00");

            SelectedLaneDisplay.Instance.m_angleDisplay.text = m_rotationGizmo.transform.GetChild(0).GetComponent<RotationGizmo>().m_currentAngle.ToString("0.00");

            //SelectedLaneDisplay.Instance.m_pivotYDisplay.text = transform.position.y.ToString("0.00");
            //SelectedLaneDisplay.Instance.m_pivotXDisplay.text = transform.position.x.ToString("0.00");
        }
    }
}
