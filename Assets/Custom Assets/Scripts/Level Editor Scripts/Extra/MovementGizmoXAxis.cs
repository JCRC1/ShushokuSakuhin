using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGizmoXAxis : MonoBehaviour
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

            main.targetObject.transform.Translate(new Vector2(mousePosition.x, 0.0f) - new Vector2(offset.x, 0.0f), main.transform);
            
            if (Input.GetKey(KeyCode.LeftControl))
            {
                float x = main.targetObject.transform.position.x;

                main.targetObject.transform.position = new Vector2(Mathf.Round(x * 4.0f) / 4.0f, main.targetObject.transform.position.y);
            }
        }
    }
}
