using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGizmoYAxis : MonoBehaviour
{
    // Reference to the main body
    public MovementGizmo m_main;

    public bool m_isDrag;
    public Vector2 m_offset;

    private void Start()
    {
        m_main = transform.parent.GetComponent<MovementGizmo>();
    }

    private void OnMouseDown()
    {
        m_isDrag = true;
        m_offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_main.m_targetObject.transform.position;
    }

    private void OnMouseUp()
    {
        m_isDrag = false;
    }

    private void Update()
    {
        if (m_isDrag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_main.m_targetObject.transform.position;

            m_main.m_targetObject.transform.Translate(new Vector2(0.0f, mousePosition.y) - new Vector2(0.0f, m_offset.y), m_main.transform);
           
            if (Input.GetKey(KeyCode.LeftControl))
            {
                float y = m_main.m_targetObject.transform.position.y;

                m_main.m_targetObject.transform.position = new Vector2(m_main.m_targetObject.transform.position.x, Mathf.Round(y * 4.0f) / 4.0f);
            }
        }
    }
}
