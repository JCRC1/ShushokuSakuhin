using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TempLaneInit : MonoBehaviour
{
    public LaneData m_tempLaneData;

    public void AddLaneInitX(Text _coord)
    {
        float.TryParse(_coord.text, out m_tempLaneData.m_initialPosition.x);
    }

    public void AddLaneInitY(Text _coord)
    {
        float.TryParse(_coord.text, out m_tempLaneData.m_initialPosition.y);
    }

    public void AddLaneInitRot(Text _rot)
    {
        m_tempLaneData.m_initialRotation = float.Parse(_rot.text);
    }

    public void CreateLane()
    {
        LaneData newLaneData = new LaneData();
        newLaneData.m_initialPosition = new Vector2(m_tempLaneData.m_initialPosition.x, m_tempLaneData.m_initialPosition.y);
        newLaneData.m_initialRotation = m_tempLaneData.m_initialRotation;
        newLaneData.m_initialAlpha = 1.0f;
        LevelEditorManager.Instance.InitEmptyLane(newLaneData);
    }
}
