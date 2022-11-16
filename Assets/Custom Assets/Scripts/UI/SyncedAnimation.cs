using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncedAnimation : MonoBehaviour
{
    //The animator controller attached to this GameObject
    public Animator animator;
    //Records the animation state or animation that the Animator is currently in
    public AnimatorStateInfo animatorStateInfo;

    //Used to address the current state within the Animator using the Play() function
    public int currentState;

    // Triggered when the game has ended
    private bool playEnd;
    // Start is called before the first frame update
    void Start()
    {
        //Load the animator attached to this object
        animator = GetComponent<Animator>();

        //Get the info about the current animator state
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Convert the current state name to an integer hash for identification
        currentState = animatorStateInfo.fullPathHash;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelEditorManager.Instance)
        {
            //Start playing the current animation from wherever the current conductor loop is
            animator.Play(currentState, -1, LevelEditorManager.Instance.loopPosInAnalog);
            //Set the speed to 0 so it will only change frames when you next update it
            animator.speed = 0;
        }
        else
        {
            if (GameManager.Instance.finalized)
            {
                animator.SetTrigger("Finalized");
                animator.speed = 1;
                return;
            }
            //Start playing the current animation from wherever the current conductor loop is
            animator.Play(currentState, -1, GameManager.Instance.loopPosInAnalog);
            //Set the speed to 0 so it will only change frames when you next update it
            animator.speed = 0;            
        }

    }
}
