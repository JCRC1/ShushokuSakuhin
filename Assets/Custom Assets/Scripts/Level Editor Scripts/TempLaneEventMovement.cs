using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempLaneEventMovement : MonoBehaviour
{
    private LaneEventMovement newMovement;

    public void MoveToTargetX(Text _toX)
    {
        float.TryParse(_toX.text, out newMovement.targetPosition.x);        
    }

    public void MoveToTargetY(Text _toY)
    {
        float.TryParse(_toY.text, out newMovement.targetPosition.y);
    }

    public void MoveToTargetDuration(Text _duration)
    {
        float.TryParse(_duration.text, out newMovement.duration);
    }

    public void MoveToBeat(Text _beat)
    {
        float.TryParse(_beat.text, out newMovement.beat);
    }
}
