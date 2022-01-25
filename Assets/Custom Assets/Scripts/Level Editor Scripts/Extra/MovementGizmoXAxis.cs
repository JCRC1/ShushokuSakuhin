using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGizmoXAxis : MonoBehaviour
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

            m_main.m_targetObject.transform.Translate(new Vector2(mousePosition.x, 0.0f) - new Vector2(m_offset.x, 0.0f), m_main.transform);
            
            if (Input.GetKey(KeyCode.LeftControl))
            {
                float x = m_main.m_targetObject.transform.position.x;

                m_main.m_targetObject.transform.position = new Vector2(Mathf.Round(x * 4.0f) / 4.0f, m_main.m_targetObject.transform.position.y);
            }
        }
    }
}
