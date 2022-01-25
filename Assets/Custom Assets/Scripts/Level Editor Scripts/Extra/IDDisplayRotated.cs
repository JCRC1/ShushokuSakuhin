using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDDisplayRotated : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.parent)
        {
            GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, -transform.parent.transform.rotation.z);
        }
    }
}
