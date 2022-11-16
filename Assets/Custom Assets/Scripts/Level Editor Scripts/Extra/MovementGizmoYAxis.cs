using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGizmoYAxis : MonoBehaviour
{
    // Reference to the main body
    public MovementGizmo main;

    public bool isDrag;
    public Vector2 offset;

    private void Start()
    {
        main = transform.parent.GetComponent<MovementGizmo>();
    }

    private void OnMouseDown()
    {
        isDrag = true;
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - main.targetObject.transform.position;
    }

    private void OnMouseUp()
    {
        isDrag = false;
    }

    private void Update()
    {
        if (isDrag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - main.targetObject.transform.position;

            main.targetObject.transform.Translate(new Vector2(0.0f, mousePosition.y) - new Vector2(0.0f, offset.y), main.transform);
           
            if (Input.GetKey(KeyCode.LeftControl))
            {
                float y = main.targetObject.transform.position.y;

                main.targetObject.transform.position = new Vector2(main.targetObject.transform.position.x, Mathf.Round(y * 4.0f) / 4.0f);
            }
        }
    }
}
