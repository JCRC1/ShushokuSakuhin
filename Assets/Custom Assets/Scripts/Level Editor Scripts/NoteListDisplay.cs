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
        public List<GameObject> objects;
        public List<SingleNoteData> single;
        public bool created;
    }

    [System.Serializable]
    public class HoldNoteItem
    {
        public List<GameObject> objects;
        public List<HoldNoteData> hold;
        public bool created;
    }

    public static NoteListDisplay Instance;

    public GameObject singleNoteDisplayTemplate;
    public GameObject holdNoteDisplayTemplate;

    public List<SingleNoteItem> singleNotes;
    public List<HoldNoteItem> holdNotes;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialized()
    {
        Instance = this;
        singleNotes = new List<SingleNoteItem>();
        holdNotes =  new List<HoldNoteItem>();
    }

    public void GenerateLoadedList()
    {
        singleNotes = new List<SingleNoteItem>();
        holdNotes = new List<HoldNoteItem>();
        // For each note type in each lane, create an item
        for (int i = 0; i < LevelEditorManager.Instance.chartData.lane.Count; i++)
        {
            // Single Notes
            if (singleNotes.Count < LevelEditorManager.Instance.chartData.lane.Count)
            {
                SingleNoteItem item = new SingleNoteItem();
                item.objects = new List<GameObject>();
                item.single = new List<SingleNoteData>();
                item.created = true;
                singleNotes.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.chartData.lane[i].singleNote.Count; j++)
            {
                if (singleNotes[i].single.Count < LevelEditorManager.Instance.chartData.lane[i].singleNote.Count)
                {
                    GameObject obj = Instantiate(singleNoteDisplayTemplate, transform);
                    obj.SetActive(true);
                    singleNotes[i].objects.Add(obj);

                    SingleNoteData note = LevelEditorManager.Instance.chartData.lane[i].singleNote[j];

                    obj.GetComponent<SingleNoteHolder>().heldNote = note;
                    singleNotes[i].single.Add(note);
                }
                singleNotes[i].objects[j].GetComponent<SingleNoteHolder>().laneID = i;
            }

            // Rotations
            if (holdNotes.Count < LevelEditorManager.Instance.chartData.lane.Count)
            {
                HoldNoteItem item = new HoldNoteItem();
                item.objects = new List<GameObject>();
                item.hold = new List<HoldNoteData>();
                item.created = true;
                holdNotes.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.chartData.lane[i].holdNote.Count; j++)
            {
                if (holdNotes[i].hold.Count < LevelEditorManager.Instance.chartData.lane[i].holdNote.Count)
                {
                    GameObject obj = Instantiate(holdNoteDisplayTemplate, transform);
                    obj.SetActive(true);
                    holdNotes[i].objects.Add(obj);

                    HoldNoteData note = LevelEditorManager.Instance.chartData.lane[i].holdNote[j];

                    obj.GetComponent<HoldNoteHolder>().heldNote = note;
                    holdNotes[i].hold.Add(note);
                }
                holdNotes[i].objects[j].GetComponent<HoldNoteHolder>().laneID = i;
            }
        }

        ListInfoDisplay();
    }

    public void ListInfoDisplay()
    {
        // Now that the things are created, lets uh....populate it?
        for (int i = 0; i < singleNotes.Count; i++)
        {
            if (singleNotes[i].single.Count > 0)
            {
                singleNotes[i].single = singleNotes[i].single.OrderBy(lst => lst.beat).ToList();
            }

            for (int j = 0; j < singleNotes[i].single.Count; j++)
            {
                if (singleNotes[i].objects[j] != null)
                {
                    foreach (Transform child in singleNotes[i].objects[j].transform)
                    {
                        if (child.childCount > 0)
                        {
                            if (child.GetComponent<Text>().text.Contains("Lane"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.lanes[i].GetComponent<LaneHandler>().identifier.ToString();
                            }
                            else if (child.GetComponent<Text>().text.Contains("Beat"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].singleNote[j].beat.ToString("0.00");
                            }
                        }
                    }
                }
            }
        }

        // And holds too
        for (int i = 0; i < holdNotes.Count; i++)
        {
            if (holdNotes[i].hold.Count > 0)
            {
                holdNotes[i].hold = holdNotes[i].hold.OrderBy(lst => lst.beat).ToList();
            }

            for (int j = 0; j < holdNotes[i].hold.Count; j++)
            {
                foreach (Transform child in holdNotes[i].objects[j].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.lanes[i].GetComponent<LaneHandler>().identifier.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].holdNote[j].beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].holdNote[j].duration.ToString("0.00");
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
        item1.objects = new List<GameObject>();
        item1.single = new List<SingleNoteData>();
        item1.created = true;
        singleNotes.Add(item1);

        HoldNoteItem item2 = new HoldNoteItem();
        item2.objects = new List<GameObject>();
        item2.hold = new List<HoldNoteData>();
        item2.created = true;
        holdNotes.Add(item2);
    }

    public void AddNoteToList(SingleNoteData _newNote, int _lane)
    {
        GameObject obj = Instantiate(singleNoteDisplayTemplate, transform);
        obj.GetComponent<SingleNoteHolder>().laneID = _lane;
        obj.GetComponent<SingleNoteHolder>().indexOfThis = LevelEditorManager.Instance.chartData.lane[_lane].singleNote.IndexOf(_newNote);
        obj.SetActive(true);

        singleNotes[_lane].objects.Insert(obj.GetComponent<SingleNoteHolder>().indexOfThis, obj);

        obj.GetComponent<SingleNoteHolder>().heldNote = _newNote;
        singleNotes[_lane].single.Insert(obj.GetComponent<SingleNoteHolder>().indexOfThis, _newNote);

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
                    child.GetChild(0).GetComponent<Text>().text = _newNote.beat.ToString("0.00");
                }
            }
        }
    }
    public void AddNoteToList(HoldNoteData _newNote, int _lane)
    {
        GameObject obj = Instantiate(holdNoteDisplayTemplate, transform);
        obj.GetComponent<HoldNoteHolder>().laneID = _lane;
        obj.GetComponent<HoldNoteHolder>().indexOfThis = LevelEditorManager.Instance.chartData.lane[_lane].holdNote.IndexOf(_newNote);
        obj.SetActive(true);

        holdNotes[_lane].objects.Insert(obj.GetComponent<HoldNoteHolder>().indexOfThis, obj);

        obj.GetComponent<HoldNoteHolder>().heldNote = _newNote;
        holdNotes[_lane].hold.Insert(obj.GetComponent<HoldNoteHolder>().indexOfThis, _newNote);

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
                    child.GetChild(0).GetComponent<Text>().text = _newNote.beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newNote.duration.ToString("0.00");
                }
            }
        }
    }

    public void EditNote(SingleNoteData _newNote, SingleNoteHolder _holder)
    {
        int lane = _holder.laneID;
        for (int i = 0; i < singleNotes[lane].objects.Count; i++)
        {
            if (_holder == singleNotes[lane].objects[i].GetComponent<SingleNoteHolder>())
            {
                foreach (Transform child in singleNotes[lane].objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newNote.beat.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
    public void EditNote(HoldNoteData _newNote, HoldNoteHolder _holder)
    {
        int lane = _holder.laneID;
        for (int i = 0; i < holdNotes[lane].objects.Count; i++)
        {
            if (_holder == holdNotes[lane].objects[i].GetComponent<HoldNoteHolder>())
            {
                foreach (Transform child in holdNotes[lane].objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newNote.beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newNote.duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
}
