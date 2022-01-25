using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempLaneEventMovement : MonoBehaviour
{
    private LaneEventMovement m_newMovement;

    public void MoveToTargetX(Text _toX)
    {
        float.TryParse(_toX.text, out m_newMovement.m_targetPosition.x);        
    }

    public void MoveToTargetY(Text _toY)
    {
        float.TryParse(_toY.text, out m_newMovement.m_targetPosition.y);
    }

    public void MoveToTargetDuration(Text _duration)
    {
        float.TryParse(_duration.text, out m_newMovement.m_duration);
    }

    public void MoveToBeat(Text _beat)
    {
        float.TryParse(_beat.text, out m_newMovement.m_beat);
    }
}
