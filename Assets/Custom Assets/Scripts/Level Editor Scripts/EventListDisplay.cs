using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EventListDisplay : MonoBehaviour
{
    [System.Serializable]
    public class MovItem
    {
        public List<GameObject> m_objects;
        public List<LaneEventMovement> m_moves;
        public bool m_created;
    }

    [System.Serializable]
    public class RotItem
    {
        public List<GameObject> m_objects;
        public List<LaneEventRotation> m_rots;
        public bool m_created;
    }

    [System.Serializable]
    public class FadeItem
    {
        public List<GameObject> m_objects;
        public List<LaneEventFade> m_fades;
        public bool m_created;
    }

    [System.Serializable]
    public class LenItem
    {
        public List<GameObject> m_objects;
        public List<LaneEventLength> m_lengths;
        public bool m_created;
    }

    public static EventListDisplay Instance;

    public GameObject m_movementTemplate;
    public GameObject m_rotationTemplate;
    public GameObject m_fadeTemplate;
    public GameObject m_lengthTemplate;

    public List<MovItem> m_movements;
    public List<RotItem> m_rotations;
    public List<FadeItem> m_fades;
    public List<LenItem> m_lengths;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_movements = new List<MovItem>();
        m_rotations = new List<RotItem>();
        m_fades = new List<FadeItem>();
        m_lengths = new List<LenItem>();
    }

    public void GenerateLoadedList()
    {
        // For each event in each lane, create an item
        for (int i = 0; i < LevelEditorManager.Instance.m_chartData.m_lane.Count; i++)
        {
            // Movements
            if (m_movements.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
            {
                MovItem item = new MovItem();
                item.m_objects = new List<GameObject>();
                item.m_moves = new List<LaneEventMovement>();
                item.m_created = true;
                m_movements.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsMovement.Count; j++)
            {
                if (m_movements[i].m_moves.Count < LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsMovement.Count)
                {
                    GameObject obj = Instantiate(m_movementTemplate, transform);
                    obj.SetActive(true);
                    m_movements[i].m_objects.Add(obj);

                    LaneEventMovement move = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsMovement[j];

                    obj.GetComponent<MoveEventHolder>().m_heldLaneEvent = move;
                    m_movements[i].m_moves.Add(move);
                }
                m_movements[i].m_objects[j].GetComponent<MoveEventHolder>().m_laneID = i;
            }

            // Rotations
            if (m_rotations.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
            {
                RotItem item = new RotItem();
                item.m_objects = new List<GameObject>();
                item.m_rots = new List<LaneEventRotation>();
                item.m_created = true;
                m_rotations.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsRotation.Count; j++)
            {
                if (m_rotations[i].m_rots.Count < LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsRotation.Count)
                {
                    GameObject obj = Instantiate(m_rotationTemplate, transform);
                    obj.SetActive(true);
                    m_rotations[i].m_objects.Add(obj);

                    LaneEventRotation rot = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsRotation[j];

                    obj.GetComponent<RotEventHolder>().m_heldLaneEvent = rot;
                    m_rotations[i].m_rots.Add(rot);
                }
                m_rotations[i].m_objects[j].GetComponent<RotEventHolder>().m_laneID = i;
            }

            // Fades
            if (m_fades.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
            {
                FadeItem item = new FadeItem();
                item.m_objects = new List<GameObject>();
                item.m_fades = new List<LaneEventFade>();
                item.m_created = true;
                m_fades.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventFade.Count; j++)
            {
                if (m_fades[i].m_fades.Count < LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventFade.Count)
                {
                    GameObject obj = Instantiate(m_fadeTemplate, transform);
                    obj.SetActive(true);
                    m_fades[i].m_objects.Add(obj);

                    LaneEventFade fade = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventFade[j];

                    obj.GetComponent<FadeEventHolder>().m_heldLaneEvent = fade;
                    m_fades[i].m_fades.Add(fade);
                }
                m_fades[i].m_objects[j].GetComponent<FadeEventHolder>().m_laneID = i;
            }

            // Lengths
            if (m_lengths.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
            {
                LenItem item = new LenItem();
                item.m_objects = new List<GameObject>();
                item.m_lengths = new List<LaneEventLength>();
                item.m_created = true;
                m_lengths.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventLength.Count; j++)
            {
                if (m_lengths[i].m_lengths.Count < LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventLength.Count)
                {
                    GameObject obj = Instantiate(m_lengthTemplate, transform);
                    obj.SetActive(true);
                    m_lengths[i].m_objects.Add(obj);

                    LaneEventLength len = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventLength[j];

                    obj.GetComponent<LengthEventHolder>().m_heldLaneEvent = len;
                    m_lengths[i].m_lengths.Add(len);
                }
                m_lengths[i].m_objects[j].GetComponent<LengthEventHolder>().m_laneID = i;
            }
        }

        ListInfoDisplay();
    }

    public void ListInfoDisplay()
    {
        // Now that the things are created, lets uh....populate it?
        for (int i = 0; i < m_movements.Count; i++)
        {
            if (m_movements[i].m_moves.Count > 0)
            {
                m_movements[i].m_moves = m_movements[i].m_moves.OrderBy(lst => lst.m_beat).ToList();
            }

            for (int j = 0; j < m_movements[i].m_moves.Count; j++)
            {
                if (m_movements[i].m_objects[j] != null)
                {
                    foreach (Transform child in m_movements[i].m_objects[j].transform)
                    {
                        if (child.childCount > 0)
                        {
                            if (child.GetComponent<Text>().text.Contains("Lane"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_lanes[i].GetComponent<LaneHandler>().m_identifier.ToString();
                            }
                            else if (child.GetComponent<Text>().text.Contains("Beat"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsMovement[j].m_beat.ToString("0.00");
                            }
                            else if (child.GetComponent<Text>().text.Contains("X"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsMovement[j].m_targetPosition.x.ToString("0.00");
                            }
                            else if (child.GetComponent<Text>().text.Contains("Y"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsMovement[j].m_targetPosition.y.ToString("0.00");
                            }
                            else if (child.GetComponent<Text>().text.Contains("Duration"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsMovement[j].m_duration.ToString("0.00");
                            }
                        }
                    }
                }
            }
        }

        // And rotations too
        for (int i = 0; i < m_rotations.Count; i++)
        {
            if (m_rotations[i].m_rots.Count > 0)
            {
                m_rotations[i].m_rots = m_rotations[i].m_rots.OrderBy(lst => lst.m_beat).ToList();
            }

            for (int j = 0; j < m_rotations[i].m_rots.Count; j++)
            {
                foreach (Transform child in m_rotations[i].m_objects[j].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_lanes[i].GetComponent<LaneHandler>().m_identifier.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsRotation[j].m_beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsRotation[j].m_targetRotation.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsRotation[j].m_duration.ToString("0.00");
                        }
                    }
                }
            }
        }

        // And fades
        for (int i = 0; i < m_fades.Count; i++)
        {
            if (m_fades[i].m_fades.Count > 0)
            {
                m_fades[i].m_fades = m_fades[i].m_fades.OrderBy(lst => lst.m_beat).ToList();
            }

            for (int j = 0; j < m_fades[i].m_fades.Count; j++)
            {
                foreach (Transform child in m_fades[i].m_objects[j].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_lanes[i].GetComponent<LaneHandler>().m_identifier.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventFade[j].m_beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventFade[j].m_targetAlpha.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventFade[j].m_duration.ToString("0.00");
                        }
                    }
                }
            }
        }

        // Aaaaand length events
        for (int i = 0; i < m_lengths.Count; i++)
        {
            if (m_lengths[i].m_lengths.Count > 0)
            {
                m_lengths[i].m_lengths = m_lengths[i].m_lengths.OrderBy(lst => lst.m_beat).ToList();
            }

            for (int j = 0; j < m_lengths[i].m_lengths.Count; j++)
            {
                foreach (Transform child in m_lengths[i].m_objects[j].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_lanes[i].GetComponent<LaneHandler>().m_identifier.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventLength[j].m_beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventLength[j].m_targetLength.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventLength[j].m_duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }

    public void AddLaneToList()
    {
        // A lane consists of 4 events, these are the 4 events
        MovItem item1 = new MovItem();
        item1.m_objects = new List<GameObject>();
        item1.m_moves = new List<LaneEventMovement>();
        item1.m_created = true;
        m_movements.Add(item1);

        RotItem item2 = new RotItem();
        item2.m_objects = new List<GameObject>();
        item2.m_rots = new List<LaneEventRotation>();
        item2.m_created = true;
        m_rotations.Add(item2);

        FadeItem item3 = new FadeItem();
        item3.m_objects = new List<GameObject>();
        item3.m_fades = new List<LaneEventFade>();
        item3.m_created = true;
        m_fades.Add(item3);

        LenItem item4 = new LenItem();
        item4.m_objects = new List<GameObject>();
        item4.m_lengths = new List<LaneEventLength>();
        item4.m_created = true;
        m_lengths.Add(item4);
    }

    public void AddEventToList(LaneEventMovement _newEvent, int _lane)
    {
        GameObject obj = Instantiate(m_movementTemplate, transform);
        obj.GetComponent<MoveEventHolder>().m_laneID = _lane;
        obj.GetComponent<MoveEventHolder>().m_indexOfThis = LevelEditorManager.Instance.m_chartData.m_lane[_lane].m_laneEventsMovement.IndexOf(_newEvent);
        obj.SetActive(true);

        m_movements[_lane].m_objects.Insert(obj.GetComponent<MoveEventHolder>().m_indexOfThis, obj);

        obj.GetComponent<MoveEventHolder>().m_heldLaneEvent = _newEvent;
        m_movements[_lane].m_moves.Insert(obj.GetComponent<MoveEventHolder>().m_indexOfThis, _newEvent);

        foreach (Transform child in obj.transform)
        {
            if (child.childCount > 0)
            {
                if (child.GetComponent<Text>().text.Contains("Lane"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _lane.ToString();
                }
                else if (child.GetComponent<Text>().text.Contains("Beat"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("X"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetPosition.x.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Y"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetPosition.y.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_duration.ToString("0.00");
                }
            }
        }
    }
    public void AddEventToList(LaneEventRotation _newEvent, int _lane)
    {
        GameObject obj = Instantiate(m_rotationTemplate, transform);
        obj.GetComponent<RotEventHolder>().m_laneID = _lane;
        obj.GetComponent<RotEventHolder>().m_indexOfThis = LevelEditorManager.Instance.m_chartData.m_lane[_lane].m_laneEventsRotation.IndexOf(_newEvent);
        obj.SetActive(true);

        m_rotations[_lane].m_objects.Insert(obj.GetComponent<RotEventHolder>().m_indexOfThis, obj);

        obj.GetComponent<RotEventHolder>().m_heldLaneEvent = _newEvent;
        m_rotations[_lane].m_rots.Insert(obj.GetComponent<RotEventHolder>().m_indexOfThis, _newEvent);

        foreach (Transform child in obj.transform)
        {
            if (child.childCount > 0)
            {
                if (child.GetComponent<Text>().text.Contains("Lane"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _lane.ToString();
                }
                else if (child.GetComponent<Text>().text.Contains("Beat"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Target"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetRotation.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_duration.ToString("0.00");
                }
            }
        }
    }
    public void AddEventToList(LaneEventFade _newEvent, int _lane)
    {
        GameObject obj = Instantiate(m_fadeTemplate, transform);
        obj.GetComponent<FadeEventHolder>().m_laneID = _lane;
        obj.GetComponent<FadeEventHolder>().m_indexOfThis = LevelEditorManager.Instance.m_chartData.m_lane[_lane].m_laneEventFade.IndexOf(_newEvent);
        obj.SetActive(true);

        m_fades[_lane].m_objects.Insert(obj.GetComponent<FadeEventHolder>().m_indexOfThis, obj);

        obj.GetComponent<FadeEventHolder>().m_heldLaneEvent = _newEvent;
        m_fades[_lane].m_fades.Insert(obj.GetComponent<FadeEventHolder>().m_indexOfThis, _newEvent);

        foreach (Transform child in obj.transform)
        {
            if (child.childCount > 0)
            {
                if (child.GetComponent<Text>().text.Contains("Lane"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _lane.ToString();
                }
                else if (child.GetComponent<Text>().text.Contains("Beat"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Target"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetAlpha.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_duration.ToString("0.00");
                }
            }
        }
    }
    public void AddEventToList(LaneEventLength _newEvent, int _lane)
    {
        GameObject obj = Instantiate(m_lengthTemplate, transform);
        obj.GetComponent<LengthEventHolder>().m_laneID = _lane;
        obj.GetComponent<LengthEventHolder>().m_indexOfThis = LevelEditorManager.Instance.m_chartData.m_lane[_lane].m_laneEventLength.IndexOf(_newEvent);
        obj.SetActive(true);

        m_lengths[_lane].m_objects.Insert(obj.GetComponent<LengthEventHolder>().m_indexOfThis, obj);

        obj.GetComponent<LengthEventHolder>().m_heldLaneEvent = _newEvent;
        m_lengths[_lane].m_lengths.Insert(obj.GetComponent<LengthEventHolder>().m_indexOfThis, _newEvent);

        foreach (Transform child in obj.transform)
        {
            if (child.childCount > 0)
            {
                if (child.GetComponent<Text>().text.Contains("Lane"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _lane.ToString();
                }
                else if (child.GetComponent<Text>().text.Contains("Beat"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Target"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetLength.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.m_duration.ToString("0.00");
                }
            }
        }
    }

    public void EditEvent(LaneEventMovement _newEvent, MoveEventHolder _holder)
    {
        int lane = _holder.m_laneID;
        for (int i = 0; i < m_movements[lane].m_objects.Count; i++)
        {
            if (_holder == m_movements[lane].m_objects[i].GetComponent<MoveEventHolder>())
            {
                foreach (Transform child in m_movements[lane].m_objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("X"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetPosition.x.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Y"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetPosition.y.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
    public void EditEvent(LaneEventRotation _newEvent, RotEventHolder _holder)
    {
        int lane = _holder.m_laneID;
        for (int i = 0; i < m_rotations[lane].m_objects.Count; i++)
        {
            if (_holder == m_rotations[lane].m_objects[i].GetComponent<RotEventHolder>())
            {
                foreach (Transform child in m_rotations[lane].m_objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetRotation.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
    public void EditEvent(LaneEventFade _newEvent, FadeEventHolder _holder)
    {
        int lane = _holder.m_laneID;
        for (int i = 0; i < m_fades[lane].m_objects.Count; i++)
        {
            if (_holder == m_fades[lane].m_objects[i].GetComponent<FadeEventHolder>())
            {
                foreach (Transform child in m_fades[lane].m_objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetAlpha.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
    public void EditEvent(LaneEventLength _newEvent, LengthEventHolder _holder)
    {
        int lane = _holder.m_laneID;
        for (int i = 0; i < m_lengths[lane].m_objects.Count; i++)
        {
            if (_holder == m_lengths[lane].m_objects[i].GetComponent<LengthEventHolder>())
            {
                foreach (Transform child in m_lengths[lane].m_objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_targetLength.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.m_duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }

}
