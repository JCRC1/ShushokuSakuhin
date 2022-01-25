using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationGizmo : MonoBehaviour
{
    public GameObject m_targetObject;

    public bool m_operating;
    public bool m_isDrag;
    private float m_offsetAngle;

    private float m_angle;
    public float m_currentAngle;            // This is the one we want
    private float m_prevAngle;
    private float m_actualAngle;

    private int m_revolutions;
    private float m_currentSign;
    private float m_prevSign;

    public Vector2 m_pivot;

    private void OnMouseDown()
    {
        m_isDrag = true;

        Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_targetObject.transform.position;
        m_offsetAngle = (Mathf.Atan2(m_targetObject.transform.right.y, m_targetObject.transform.right.x) - Mathf.Atan2(vec.y, vec.x)) * Mathf.Rad2Deg;
    }

    private void OnMouseUp()
    {
        m_isDrag = false;
        m_revolutions = 0;
    }

    public void SetTargetObject(GameObject _target)
    {
        m_targetObject = _target;
    }

    private void Update()
    {
        m_prevAngle = m_angle;
        m_prevSign = m_currentSign;

        if (m_operating && m_targetObject)
        {
            transform.parent.transform.position = m_targetObject.transform.position;
            transform.position = m_targetObject.transform.position;
            transform.parent.transform.rotation = m_targetObject.transform.rotation;
        }

        if (m_isDrag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_targetObject.transform.position;

            //m_angle = Vector2.SignedAngle(mousePosition, Vector2.up);
            m_angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
            m_currentSign = Vector2.Dot(mousePosition, Vector2.up);

            if (m_angle < 0)
            {
                m_angle = 360 - m_angle * -1;
            }

            if (m_prevAngle > 270 && m_prevAngle < 360 && m_prevSign < 0 && m_currentSign > 0)
            {
                m_revolutions++;
            }
            if (m_prevAngle < 90 && m_prevAngle > 0 && m_prevSign > 0 && m_currentSign < 0)
            {
                m_revolutions--;
            }

            m_currentAngle = m_targetObject.transform.eulerAngles.z + 360 * m_revolutions;
            m_actualAngle = m_angle + 360 * m_revolutions;

            //m_targetObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_actualAngle + m_offsetAngle);

            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, m_actualAngle + m_offsetAngle);
            //m_targetObject.transform.position = rot * (m_targetObject.transform.position - (Vector3)m_pivot) + (Vector3)m_pivot;
            m_targetObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_actualAngle + m_offsetAngle);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                float z = m_targetObject.transform.eulerAngles.z;

                m_targetObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Round(z / 15.0f) * 15));
            }
        }
    }
}
