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
            if (LevelEditorManager.Instance.trackPos > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, LevelEditorManager.Instance.loopPosInAnalog));
            }
        }
        else
        {
            if (GameManager.Instance.trackPos > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, GameManager.Instance.loopPosInAnalog));
            }
            if (GameManager.Instance.finalized)
            {
                GetComponent<Animator>().SetTrigger("Finalized");
            }
        }
    }
}
