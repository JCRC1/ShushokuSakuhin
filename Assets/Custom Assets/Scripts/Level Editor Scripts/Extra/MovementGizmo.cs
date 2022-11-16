using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGizmo : MonoBehaviour
{
    public GameObject targetObject;

    public bool operating;
    public bool xOnly;

    public bool isDrag;
    public Vector2 offset;

    private void OnMouseDown()
    {
        isDrag = true;
        if(!xOnly)
            offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - targetObject.transform.position;
        else
            offset = targetObject.transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) - targetObject.transform.position;
    }

    private void OnMouseUp()
    {
        isDrag = false;
    }


    private void Update()
    {
        if (operating && targetObject)
        {
            transform.position = targetObject.transform.position;
        }

        if (isDrag)
        {
            if(!xOnly)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - targetObject.transform.position;
                targetObject.transform.Translate(mousePosition - offset, transform);
            }
            else
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - targetObject.transform.position;
                targetObject.transform.Translate(targetObject.transform.parent.InverseTransformPoint(mousePosition) - (Vector3)offset, transform);
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                float x = targetObject.transform.position.x;
                float y = targetObject.transform.position.y;

                targetObject.transform.position = new Vector2(Mathf.Round(x * 4.0f) / 4.0f, Mathf.Round(y * 4.0f) / 4.0f);
            }
        }
    }

    public void SetTargetObject(GameObject _target)
    {
        targetObject = _target;
    }
}
