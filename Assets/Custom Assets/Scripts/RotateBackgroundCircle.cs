using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBackgroundCircle : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (LevelEditorManager.Instance)
        {
            if (LevelEditorManager.Instance.m_trackPos > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, LevelEditorManager.Instance.m_loopPosInAnalog / 16));
            }
        }
        else
        {
            if (GameManager.Instance.m_trackPos > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, GameManager.Instance.m_loopPosInAnalog / 16));
            }
        }

        if (GameManager.Instance.m_finalized)
        {
            GetComponent<Animator>().SetTrigger("Finalized");
        }
    }
}
