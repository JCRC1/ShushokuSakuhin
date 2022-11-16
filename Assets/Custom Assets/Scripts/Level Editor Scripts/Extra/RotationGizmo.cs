using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationGizmo : MonoBehaviour
{
    public GameObject targetObject;

    public bool operating;
    public bool isDrag;
    private float offsetAngle;

    private float angle;
    public float currentAngle;            // This is the one we want
    private float prevAngle;
    private float actualAngle;

    private int revolutions;
    private float currentSign;
    private float prevSign;

    public Vector2 pivot;

    private void OnMouseDown()
    {
        isDrag = true;

        Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - targetObject.transform.position;
        offsetAngle = (Mathf.Atan2(targetObject.transform.right.y, targetObject.transform.right.x) - Mathf.Atan2(vec.y, vec.x)) * Mathf.Rad2Deg;
    }

    private void OnMouseUp()
    {
        isDrag = false;
        revolutions = 0;
    }

    public void SetTargetObject(GameObject _target)
    {
        targetObject = _target;
    }

    private void Update()
    {
        prevAngle = angle;
        prevSign = currentSign;

        if (operating && targetObject)
        {
            transform.parent.transform.position = targetObject.transform.position;
            transform.position = targetObject.transform.position;
            transform.parent.transform.rotation = targetObject.transform.rotation;
        }

        if (isDrag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - targetObject.transform.position;

            //angle = Vector2.SignedAngle(mousePosition, Vector2.up);
            angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
            currentSign = Vector2.Dot(mousePosition, Vector2.up);

            if (angle < 0)
            {
                angle = 360 - angle * -1;
            }

            if (prevAngle > 270 && prevAngle < 360 && prevSign < 0 && currentSign > 0)
            {
                revolutions++;
            }
            if (prevAngle < 90 && prevAngle > 0 && prevSign > 0 && currentSign < 0)
            {
                revolutions--;
            }

            currentAngle = targetObject.transform.eulerAngles.z + 360 * revolutions;
            actualAngle = angle + 360 * revolutions;

            //targetObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, actualAngle + offsetAngle);

            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, actualAngle + offsetAngle);
            //targetObject.transform.position = rot * (targetObject.transform.position - (Vector3)pivot) + (Vector3)pivot;
            targetObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, actualAngle + offsetAngle);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                float z = targetObject.transform.eulerAngles.z;

                targetObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Round(z / 15.0f) * 15));
            }
        }
    }
}
