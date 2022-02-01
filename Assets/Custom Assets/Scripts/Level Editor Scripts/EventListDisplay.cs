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

    public class RotItem
    {
        public List<GameObject> m_objects;
        public List<LaneEventRotation> m_rots;
        public bool m_created;
    }

    public class FadeItem
    {
        public List<GameObject> m_objects;
        public List<LaneEventFade> m_fades;
        public bool m_created;
    }

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

    public List<int> m_movementIndex;
    public List<int> m_rotationIndex;
    public List<int> m_fadeIndex;
    public List<int> m_lengthIndex;

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

        m_rotationIndex = new List<int>();
        m_movementIndex = new List<int>();
        m_fadeIndex = new List<int>();
        m_lengthIndex = new List<int>();
    }

    private void Update()
    {
        if (LevelEditorManager.Instance.m_initialized)
        {
            // For each event in each lane, create an item
            for (int i = 0; i < LevelEditorManager.Instance.m_chartData.m_lane.Count; i++)
            {
                // Movements
                if (m_movements.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
                {
                    m_movementIndex.Add(0);
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

                        LaneEventMovement move = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsMovement[m_movementIndex[i]];

                        obj.GetComponent<MoveEventHolder>().m_heldLaneEvent = move;
                        m_movements[i].m_moves.Add(move);
                        m_movementIndex[i]++;
                    }
                    m_movements[i].m_objects[j].GetComponent<MoveEventHolder>().m_laneID = i;
                    m_movements[i].m_objects[j].GetComponent<MoveEventHolder>().m_indexOfThis = j;
                }

                // Rotations
                if (m_rotations.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
                {
                    m_rotationIndex.Add(0);
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

                        LaneEventRotation rot = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventsRotation[m_rotationIndex[i]];

                        obj.GetComponent<RotEventHolder>().m_heldLaneEvent = rot;                        
                        m_rotations[i].m_rots.Add(rot);
                        m_rotationIndex[i]++;
                    }
                    m_rotations[i].m_objects[j].GetComponent<RotEventHolder>().m_laneID = i;
                    m_rotations[i].m_objects[j].GetComponent<RotEventHolder>().m_indexOfThis = j;
                }

                // Fades
                if (m_fades.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
                {
                    m_fadeIndex.Add(0);
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

                        LaneEventFade fade = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventFade[m_fadeIndex[i]];

                        obj.GetComponent<FadeEventHolder>().m_heldLaneEvent = fade;
                        m_fades[i].m_fades.Add(fade);
                        m_fadeIndex[i]++;
                    }
                    m_fades[i].m_objects[j].GetComponent<FadeEventHolder>().m_laneID = i;
                    m_fades[i].m_objects[j].GetComponent<FadeEventHolder>().m_indexOfThis = j;
                }

                // Lengths
                if (m_lengths.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
                {
                    m_lengthIndex.Add(0);
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

                        LaneEventLength len = LevelEditorManager.Instance.m_chartData.m_lane[i].m_laneEventLength[m_lengthIndex[i]];

                        obj.GetComponent<LengthEventHolder>().m_heldLaneEvent = len;
                        m_lengths[i].m_lengths.Add(len);
                        m_lengthIndex[i]++;
                    }
                    m_lengths[i].m_objects[j].GetComponent<LengthEventHolder>().m_laneID = i;
                    m_lengths[i].m_objects[j].GetComponent<LengthEventHolder>().m_indexOfThis = j;
                }
            }

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
    }
}
