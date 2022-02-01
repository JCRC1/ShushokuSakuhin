using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGizmo : MonoBehaviour
{
    public GameObject m_targetObject;

    public bool m_operating;
    public bool m_xOnly;

    public bool m_isDrag;
    public Vector2 m_offset;

    private void OnMouseDown()
    {
        m_isDrag = true;
        if(!m_xOnly)
            m_offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_targetObject.transform.position;
        else
            m_offset = m_targetObject.transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) - m_targetObject.transform.position;
    }

    private void OnMouseUp()
    {
        m_isDrag = false;
    }


    private void Update()
    {
        if (m_operating && m_targetObject)
        {
            transform.position = m_targetObject.transform.position;
        }

        if (m_isDrag)
        {
            if(!m_xOnly)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_targetObject.transform.position;
                m_targetObject.transform.Translate(mousePosition - m_offset, transform);
            }
            else
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_targetObject.transform.position;
                m_targetObject.transform.Translate(m_targetObject.transform.parent.InverseTransformPoint(mousePosition) - (Vector3)m_offset, transform);
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                float x = m_targetObject.transform.position.x;
                float y = m_targetObject.transform.position.y;

                m_targetObject.transform.position = new Vector2(Mathf.Round(x * 4.0f) / 4.0f, Mathf.Round(y * 4.0f) / 4.0f);
            }
        }
    }

    public void SetTargetObject(GameObject _target)
    {
        m_targetObject = _target;
    }
}
