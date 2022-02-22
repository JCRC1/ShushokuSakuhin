using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NoteListDisplay : MonoBehaviour
{
    [System.Serializable]
    public class SingleNoteItem
    {
        public List<GameObject> m_objects;
        public List<SingleNoteData> m_single;
        public bool m_created;
    }

    [System.Serializable]
    public class HoldNoteItem
    {
        public List<GameObject> m_objects;
        public List<HoldNoteData> m_hold;
        public bool m_created;
    }

    public static NoteListDisplay Instance;

    public GameObject m_singleNoteDisplayTemplate;
    public GameObject m_holdNoteDisplayTemplate;

    public List<SingleNoteItem> m_singleNotes;
    public List<HoldNoteItem> m_holdNotes;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialized()
    {
        Instance = this;
        m_singleNotes = new List<SingleNoteItem>();
        m_holdNotes =  new List<HoldNoteItem>();
    }

    public void GenerateLoadedList()
    {
        m_singleNotes = new List<SingleNoteItem>();
        m_holdNotes = new List<HoldNoteItem>();
        // For each note type in each lane, create an item
        for (int i = 0; i < LevelEditorManager.Instance.m_chartData.m_lane.Count; i++)
        {
            // Single Notes
            if (m_singleNotes.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
            {
                SingleNoteItem item = new SingleNoteItem();
                item.m_objects = new List<GameObject>();
                item.m_single = new List<SingleNoteData>();
                item.m_created = true;
                m_singleNotes.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.m_chartData.m_lane[i].m_singleNote.Count; j++)
            {
                if (m_singleNotes[i].m_single.Count < LevelEditorManager.Instance.m_chartData.m_lane[i].m_singleNote.Count)
                {
                    GameObject obj = Instantiate(m_singleNoteDisplayTemplate, transform);
                    obj.SetActive(true);
                    m_singleNotes[i].m_objects.Add(obj);

                    SingleNoteData note = LevelEditorManager.Instance.m_chartData.m_lane[i].m_singleNote[j];

                    obj.GetComponent<SingleNoteHolder>().m_heldNote = note;
                    m_singleNotes[i].m_single.Add(note);
                }
                m_singleNotes[i].m_objects[j].GetComponent<SingleNoteHolder>().m_laneID = i;
            }

            // Rotations
            if (m_holdNotes.Count < LevelEditorManager.Instance.m_chartData.m_lane.Count)
            {
                HoldNoteItem item = new HoldNoteItem();
                item.m_objects = new List<GameObject>();
                item.m_hold = new List<HoldNoteData>();
                item.m_created = true;
                m_holdNotes.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.m_chartData.m_lane[i].m_holdNote.Count; j++)
            {
                if (m_holdNotes[i].m_hold.Count < LevelEditorManager.Instance.m_chartData.m_lane[i].m_holdNote.Count)
                {
                    GameObject obj = Instantiate(m_holdNoteDisplayTemplate, transform);
                    obj.SetActive(true);
                    m_holdNotes[i].m_objects.Add(obj);

                    HoldNoteData note = LevelEditorManager.Instance.m_chartData.m_lane[i].m_holdNote[j];

                    obj.GetComponent<HoldNoteHolder>().m_heldNote = note;
                    m_holdNotes[i].m_hold.Add(note);
                }
                m_holdNotes[i].m_objects[j].GetComponent<HoldNoteHolder>().m_laneID = i;
            }
        }

        ListInfoDisplay();
    }

    public void ListInfoDisplay()
    {
        // Now that the things are created, lets uh....populate it?
        for (int i = 0; i < m_singleNotes.Count; i++)
        {
            if (m_singleNotes[i].m_single.Count > 0)
            {
                m_singleNotes[i].m_single = m_singleNotes[i].m_single.OrderBy(lst => lst.m_beat).ToList();
            }

            for (int j = 0; j < m_singleNotes[i].m_single.Count; j++)
            {
                if (m_singleNotes[i].m_objects[j] != null)
                {
                    foreach (Transform child in m_singleNotes[i].m_objects[j].transform)
                    {
                        if (child.childCount > 0)
                        {
                            if (child.GetComponent<Text>().text.Contains("Lane"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_lanes[i].GetComponent<LaneHandler>().m_identifier.ToString();
                            }
                            else if (child.GetComponent<Text>().text.Contains("Beat"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_singleNote[j].m_beat.ToString("0.00");
                            }
                        }
                    }
                }
            }
        }

        // And holds too
        for (int i = 0; i < m_holdNotes.Count; i++)
        {
            if (m_holdNotes[i].m_hold.Count > 0)
            {
                m_holdNotes[i].m_hold = m_holdNotes[i].m_hold.OrderBy(lst => lst.m_beat).ToList();
            }

            for (int j = 0; j < m_holdNotes[i].m_hold.Count; j++)
            {
                foreach (Transform child in m_holdNotes[i].m_objects[j].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_lanes[i].GetComponent<LaneHandler>().m_identifier.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_holdNote[j].m_beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.m_chartData.m_lane[i].m_holdNote[j].m_duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }

    public void AddLaneToList()
    {
        // A lane contains of 2 lists of notes
        SingleNoteItem item1 = new SingleNoteItem();
        item1.m_objects = new List<GameObject>();
        item1.m_single = new List<SingleNoteData>();
        item1.m_created = true;
        m_singleNotes.Add(item1);

        HoldNoteItem item2 = new HoldNoteItem();
        item2.m_objects = new List<GameObject>();
        item2.m_hold = new List<HoldNoteData>();
        item2.m_created = true;
        m_holdNotes.Add(item2);
    }

    public void AddNoteToList(SingleNoteData _newNote, int _lane)
    {
        GameObject obj = Instantiate(m_singleNoteDisplayTemplate, transform);
        obj.GetComponent<SingleNoteHolder>().m_laneID = _lane;
        obj.SetActive(true);

        m_singleNotes[_lane].m_objects.Add(obj);

        obj.GetComponent<SingleNoteHolder>().m_heldNote = _newNote;
        m_singleNotes[_lane].m_single.Insert(obj.GetComponent<SingleNoteHolder>().m_indexOfThis, _newNote);

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
                    child.GetChild(0).GetComponent<Text>().text = _newNote.m_beat.ToString("0.00");
                }
            }
        }
    }
    public void AddNoteToList(HoldNoteData _newNote, int _lane)
    {
        GameObject obj = Instantiate(m_holdNoteDisplayTemplate, transform);
        obj.GetComponent<HoldNoteHolder>().m_laneID = _lane;
        obj.SetActive(true);

        m_holdNotes[_lane].m_objects.Add(obj);

        obj.GetComponent<HoldNoteHolder>().m_heldNote = _newNote;
        m_holdNotes[_lane].m_hold.Insert(obj.GetComponent<HoldNoteHolder>().m_indexOfThis, _newNote);

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
                    child.GetChild(0).GetComponent<Text>().text = _newNote.m_beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newNote.m_duration.ToString("0.00");
                }
            }
        }
    }

    public void EditNote(SingleNoteData _newNote, SingleNoteHolder _holder)
    {
        int lane = _holder.m_laneID;
        for (int i = 0; i < m_singleNotes[lane].m_objects.Count; i++)
        {
            if (_holder == m_singleNotes[lane].m_objects[i].GetComponent<SingleNoteHolder>())
            {
                foreach (Transform child in m_singleNotes[lane].m_objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newNote.m_beat.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
    public void EditNote(HoldNoteData _newNote, HoldNoteHolder _holder)
    {
        int lane = _holder.m_laneID;
        for (int i = 0; i < m_holdNotes[lane].m_objects.Count; i++)
        {
            if (_holder == m_holdNotes[lane].m_objects[i].GetComponent<HoldNoteHolder>())
            {
                foreach (Transform child in m_holdNotes[lane].m_objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newNote.m_beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newNote.m_duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
}
