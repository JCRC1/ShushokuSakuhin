using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TempLaneInit : MonoBehaviour
{
    public LaneData tempLaneData;

    public void AddLaneInitX(Text _coord)
    {
        float.TryParse(_coord.text, out tempLaneData.initialPosition.x);
    }

    public void AddLaneInitY(Text _coord)
    {
        float.TryParse(_coord.text, out tempLaneData.initialPosition.y);
    }

    public void AddLaneInitRot(Text _rot)
    {
        tempLaneData.initialRotation = float.Parse(_rot.text);
    }

    public void CreateLane()
    {
        LaneData newLaneData = new LaneData();
        newLaneData.initialPosition = new Vector2(tempLaneData.initialPosition.x, tempLaneData.initialPosition.y);
        newLaneData.initialRotation = tempLaneData.initialRotation;
        newLaneData.initialAlpha = 1.0f;
        LevelEditorManager.Instance.InitEmptyLane(newLaneData);
        
        EventListDisplay.Instance.AddLaneToList();

        if(NoteListDisplay.Instance)
        {
            NoteListDisplay.Instance.AddLaneToList();
        }
        else
        {
            Resources.FindObjectsOfTypeAll<NoteListDisplay>()[0].Initialized();
            Resources.FindObjectsOfTypeAll<NoteListDisplay>()[0].AddLaneToList();
        }        
    }
}
