using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncedAnimation : MonoBehaviour
{
    //The animator controller attached to this GameObject
    public Animator m_animator;
    //Records the animation state or animation that the Animator is currently in
    public AnimatorStateInfo m_animatorStateInfo;

    //Used to address the current state within the Animator using the Play() function
    public int m_currentState;

    // Triggered when the game has ended
    private bool m_playEnd;
    // Start is called before the first frame update
    void Start()
    {
        //Load the animator attached to this object
        m_animator = GetComponent<Animator>();

        //Get the info about the current animator state
        m_animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        //Convert the current state name to an integer hash for identification
        m_currentState = m_animatorStateInfo.fullPathHash;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelEditorManager.Instance)
        {
            //Start playing the current animation from wherever the current conductor loop is
            m_animator.Play(m_currentState, -1, LevelEditorManager.Instance.m_loopPosInAnalog);
            //Set the speed to 0 so it will only change frames when you next update it
            m_animator.speed = 0;
        }
        else
        {
            if (GameManager.Instance.m_finalized)
            {
                m_animator.SetTrigger("Finalized");
                m_animator.speed = 1;
                return;
            }
            //Start playing the current animation from wherever the current conductor loop is
            m_animator.Play(m_currentState, -1, GameManager.Instance.m_loopPosInAnalog);
            //Set the speed to 0 so it will only change frames when you next update it
            m_animator.speed = 0;            
        }

    }
}
