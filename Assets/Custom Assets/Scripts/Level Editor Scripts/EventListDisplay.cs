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
        public List<GameObject> objects;
        public List<LaneEventMovement> moves;
        public bool created;
    }

    [System.Serializable]
    public class RotItem
    {
        public List<GameObject> objects;
        public List<LaneEventRotation> rots;
        public bool created;
    }

    [System.Serializable]
    public class FadeItem
    {
        public List<GameObject> objects;
        public List<LaneEventFade> fades;
        public bool created;
    }

    [System.Serializable]
    public class LenItem
    {
        public List<GameObject> objects;
        public List<LaneEventLength> lengths;
        public bool created;
    }

    public static EventListDisplay Instance;

    public GameObject movementTemplate;
    public GameObject rotationTemplate;
    public GameObject fadeTemplate;
    public GameObject lengthTemplate;

    public List<MovItem> movements;
    public List<RotItem> rotations;
    public List<FadeItem> fades;
    public List<LenItem> lengths;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        movements = new List<MovItem>();
        rotations = new List<RotItem>();
        fades = new List<FadeItem>();
        lengths = new List<LenItem>();
    }

    public void GenerateLoadedList()
    {
        // For each event in each lane, create an item
        for (int i = 0; i < LevelEditorManager.Instance.chartData.lane.Count; i++)
        {
            // Movements
            if (movements.Count < LevelEditorManager.Instance.chartData.lane.Count)
            {
                MovItem item = new MovItem();
                item.objects = new List<GameObject>();
                item.moves = new List<LaneEventMovement>();
                item.created = true;
                movements.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.chartData.lane[i].laneEventsMovement.Count; j++)
            {
                if (movements[i].moves.Count < LevelEditorManager.Instance.chartData.lane[i].laneEventsMovement.Count)
                {
                    GameObject obj = Instantiate(movementTemplate, transform);
                    obj.SetActive(true);
                    movements[i].objects.Add(obj);

                    LaneEventMovement move = LevelEditorManager.Instance.chartData.lane[i].laneEventsMovement[j];

                    obj.GetComponent<MoveEventHolder>().heldLaneEvent = move;
                    movements[i].moves.Add(move);
                }
                movements[i].objects[j].GetComponent<MoveEventHolder>().laneID = i;
            }

            // Rotations
            if (rotations.Count < LevelEditorManager.Instance.chartData.lane.Count)
            {
                RotItem item = new RotItem();
                item.objects = new List<GameObject>();
                item.rots = new List<LaneEventRotation>();
                item.created = true;
                rotations.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.chartData.lane[i].laneEventsRotation.Count; j++)
            {
                if (rotations[i].rots.Count < LevelEditorManager.Instance.chartData.lane[i].laneEventsRotation.Count)
                {
                    GameObject obj = Instantiate(rotationTemplate, transform);
                    obj.SetActive(true);
                    rotations[i].objects.Add(obj);

                    LaneEventRotation rot = LevelEditorManager.Instance.chartData.lane[i].laneEventsRotation[j];

                    obj.GetComponent<RotEventHolder>().heldLaneEvent = rot;
                    rotations[i].rots.Add(rot);
                }
                rotations[i].objects[j].GetComponent<RotEventHolder>().laneID = i;
            }

            // Fades
            if (fades.Count < LevelEditorManager.Instance.chartData.lane.Count)
            {
                FadeItem item = new FadeItem();
                item.objects = new List<GameObject>();
                item.fades = new List<LaneEventFade>();
                item.created = true;
                fades.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.chartData.lane[i].laneEventFade.Count; j++)
            {
                if (fades[i].fades.Count < LevelEditorManager.Instance.chartData.lane[i].laneEventFade.Count)
                {
                    GameObject obj = Instantiate(fadeTemplate, transform);
                    obj.SetActive(true);
                    fades[i].objects.Add(obj);

                    LaneEventFade fade = LevelEditorManager.Instance.chartData.lane[i].laneEventFade[j];

                    obj.GetComponent<FadeEventHolder>().heldLaneEvent = fade;
                    fades[i].fades.Add(fade);
                }
                fades[i].objects[j].GetComponent<FadeEventHolder>().laneID = i;
            }

            // Lengths
            if (lengths.Count < LevelEditorManager.Instance.chartData.lane.Count)
            {
                LenItem item = new LenItem();
                item.objects = new List<GameObject>();
                item.lengths = new List<LaneEventLength>();
                item.created = true;
                lengths.Add(item);
            }

            for (int j = 0; j < LevelEditorManager.Instance.chartData.lane[i].laneEventLength.Count; j++)
            {
                if (lengths[i].lengths.Count < LevelEditorManager.Instance.chartData.lane[i].laneEventLength.Count)
                {
                    GameObject obj = Instantiate(lengthTemplate, transform);
                    obj.SetActive(true);
                    lengths[i].objects.Add(obj);

                    LaneEventLength len = LevelEditorManager.Instance.chartData.lane[i].laneEventLength[j];

                    obj.GetComponent<LengthEventHolder>().heldLaneEvent = len;
                    lengths[i].lengths.Add(len);
                }
                lengths[i].objects[j].GetComponent<LengthEventHolder>().laneID = i;
            }
        }

        ListInfoDisplay();
    }

    public void ListInfoDisplay()
    {
        // Now that the things are created, lets uh....populate it?
        for (int i = 0; i < movements.Count; i++)
        {
            if (movements[i].moves.Count > 0)
            {
                movements[i].moves = movements[i].moves.OrderBy(lst => lst.beat).ToList();
            }

            for (int j = 0; j < movements[i].moves.Count; j++)
            {
                if (movements[i].objects[j] != null)
                {
                    foreach (Transform child in movements[i].objects[j].transform)
                    {
                        if (child.childCount > 0)
                        {
                            if (child.GetComponent<Text>().text.Contains("Lane"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.lanes[i].GetComponent<LaneHandler>().identifier.ToString();
                            }
                            else if (child.GetComponent<Text>().text.Contains("Beat"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventsMovement[j].beat.ToString("0.00");
                            }
                            else if (child.GetComponent<Text>().text.Contains("X"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventsMovement[j].targetPosition.x.ToString("0.00");
                            }
                            else if (child.GetComponent<Text>().text.Contains("Y"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventsMovement[j].targetPosition.y.ToString("0.00");
                            }
                            else if (child.GetComponent<Text>().text.Contains("Duration"))
                            {
                                child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventsMovement[j].duration.ToString("0.00");
                            }
                        }
                    }
                }
            }
        }

        // And rotations too
        for (int i = 0; i < rotations.Count; i++)
        {
            if (rotations[i].rots.Count > 0)
            {
                rotations[i].rots = rotations[i].rots.OrderBy(lst => lst.beat).ToList();
            }

            for (int j = 0; j < rotations[i].rots.Count; j++)
            {
                foreach (Transform child in rotations[i].objects[j].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.lanes[i].GetComponent<LaneHandler>().identifier.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventsRotation[j].beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventsRotation[j].targetRotation.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventsRotation[j].duration.ToString("0.00");
                        }
                    }
                }
            }
        }

        // And fades
        for (int i = 0; i < fades.Count; i++)
        {
            if (fades[i].fades.Count > 0)
            {
                fades[i].fades = fades[i].fades.OrderBy(lst => lst.beat).ToList();
            }

            for (int j = 0; j < fades[i].fades.Count; j++)
            {
                foreach (Transform child in fades[i].objects[j].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.lanes[i].GetComponent<LaneHandler>().identifier.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventFade[j].beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventFade[j].targetAlpha.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventFade[j].duration.ToString("0.00");
                        }
                    }
                }
            }
        }

        // Aaaaand length events
        for (int i = 0; i < lengths.Count; i++)
        {
            if (lengths[i].lengths.Count > 0)
            {
                lengths[i].lengths = lengths[i].lengths.OrderBy(lst => lst.beat).ToList();
            }

            for (int j = 0; j < lengths[i].lengths.Count; j++)
            {
                foreach (Transform child in lengths[i].objects[j].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.lanes[i].GetComponent<LaneHandler>().identifier.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventLength[j].beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventLength[j].targetLength.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = LevelEditorManager.Instance.chartData.lane[i].laneEventLength[j].duration.ToString("0.00");
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
        item1.objects = new List<GameObject>();
        item1.moves = new List<LaneEventMovement>();
        item1.created = true;
        movements.Add(item1);

        RotItem item2 = new RotItem();
        item2.objects = new List<GameObject>();
        item2.rots = new List<LaneEventRotation>();
        item2.created = true;
        rotations.Add(item2);

        FadeItem item3 = new FadeItem();
        item3.objects = new List<GameObject>();
        item3.fades = new List<LaneEventFade>();
        item3.created = true;
        fades.Add(item3);

        LenItem item4 = new LenItem();
        item4.objects = new List<GameObject>();
        item4.lengths = new List<LaneEventLength>();
        item4.created = true;
        lengths.Add(item4);
    }

    public void AddEventToList(LaneEventMovement _newEvent, int _lane)
    {
        GameObject obj = Instantiate(movementTemplate, transform);
        obj.GetComponent<MoveEventHolder>().laneID = _lane;
        obj.GetComponent<MoveEventHolder>().indexOfThis = LevelEditorManager.Instance.chartData.lane[_lane].laneEventsMovement.IndexOf(_newEvent);
        obj.SetActive(true);

        movements[_lane].objects.Insert(obj.GetComponent<MoveEventHolder>().indexOfThis, obj);

        obj.GetComponent<MoveEventHolder>().heldLaneEvent = _newEvent;
        movements[_lane].moves.Insert(obj.GetComponent<MoveEventHolder>().indexOfThis, _newEvent);

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
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("X"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.targetPosition.x.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Y"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.targetPosition.y.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.duration.ToString("0.00");
                }
            }
        }
    }
    public void AddEventToList(LaneEventRotation _newEvent, int _lane)
    {
        GameObject obj = Instantiate(rotationTemplate, transform);
        obj.GetComponent<RotEventHolder>().laneID = _lane;
        obj.GetComponent<RotEventHolder>().indexOfThis = LevelEditorManager.Instance.chartData.lane[_lane].laneEventsRotation.IndexOf(_newEvent);
        obj.SetActive(true);

        rotations[_lane].objects.Insert(obj.GetComponent<RotEventHolder>().indexOfThis, obj);

        obj.GetComponent<RotEventHolder>().heldLaneEvent = _newEvent;
        rotations[_lane].rots.Insert(obj.GetComponent<RotEventHolder>().indexOfThis, _newEvent);

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
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Target"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.targetRotation.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.duration.ToString("0.00");
                }
            }
        }
    }
    public void AddEventToList(LaneEventFade _newEvent, int _lane)
    {
        GameObject obj = Instantiate(fadeTemplate, transform);
        obj.GetComponent<FadeEventHolder>().laneID = _lane;
        obj.GetComponent<FadeEventHolder>().indexOfThis = LevelEditorManager.Instance.chartData.lane[_lane].laneEventFade.IndexOf(_newEvent);
        obj.SetActive(true);

        fades[_lane].objects.Insert(obj.GetComponent<FadeEventHolder>().indexOfThis, obj);

        obj.GetComponent<FadeEventHolder>().heldLaneEvent = _newEvent;
        fades[_lane].fades.Insert(obj.GetComponent<FadeEventHolder>().indexOfThis, _newEvent);

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
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Target"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.targetAlpha.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.duration.ToString("0.00");
                }
            }
        }
    }
    public void AddEventToList(LaneEventLength _newEvent, int _lane)
    {
        GameObject obj = Instantiate(lengthTemplate, transform);
        obj.GetComponent<LengthEventHolder>().laneID = _lane;
        obj.GetComponent<LengthEventHolder>().indexOfThis = LevelEditorManager.Instance.chartData.lane[_lane].laneEventLength.IndexOf(_newEvent);
        obj.SetActive(true);

        lengths[_lane].objects.Insert(obj.GetComponent<LengthEventHolder>().indexOfThis, obj);

        obj.GetComponent<LengthEventHolder>().heldLaneEvent = _newEvent;
        lengths[_lane].lengths.Insert(obj.GetComponent<LengthEventHolder>().indexOfThis, _newEvent);

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
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.beat.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Target"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.targetLength.ToString("0.00");
                }
                else if (child.GetComponent<Text>().text.Contains("Duration"))
                {
                    child.GetChild(0).GetComponent<Text>().text = _newEvent.duration.ToString("0.00");
                }
            }
        }
    }

    public void EditEvent(LaneEventMovement _newEvent, MoveEventHolder _holder)
    {
        int lane = _holder.laneID;
        for (int i = 0; i < movements[lane].objects.Count; i++)
        {
            if (_holder == movements[lane].objects[i].GetComponent<MoveEventHolder>())
            {
                foreach (Transform child in movements[lane].objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("X"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.targetPosition.x.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Y"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.targetPosition.y.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
    public void EditEvent(LaneEventRotation _newEvent, RotEventHolder _holder)
    {
        int lane = _holder.laneID;
        for (int i = 0; i < rotations[lane].objects.Count; i++)
        {
            if (_holder == rotations[lane].objects[i].GetComponent<RotEventHolder>())
            {
                foreach (Transform child in rotations[lane].objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.targetRotation.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
    public void EditEvent(LaneEventFade _newEvent, FadeEventHolder _holder)
    {
        int lane = _holder.laneID;
        for (int i = 0; i < fades[lane].objects.Count; i++)
        {
            if (_holder == fades[lane].objects[i].GetComponent<FadeEventHolder>())
            {
                foreach (Transform child in fades[lane].objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.targetAlpha.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }
    public void EditEvent(LaneEventLength _newEvent, LengthEventHolder _holder)
    {
        int lane = _holder.laneID;
        for (int i = 0; i < lengths[lane].objects.Count; i++)
        {
            if (_holder == lengths[lane].objects[i].GetComponent<LengthEventHolder>())
            {
                foreach (Transform child in lengths[lane].objects[i].transform)
                {
                    if (child.childCount > 0)
                    {
                        if (child.GetComponent<Text>().text.Contains("Lane"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = lane.ToString();
                        }
                        else if (child.GetComponent<Text>().text.Contains("Beat"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.beat.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Target"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.targetLength.ToString("0.00");
                        }
                        else if (child.GetComponent<Text>().text.Contains("Duration"))
                        {
                            child.GetChild(0).GetComponent<Text>().text = _newEvent.duration.ToString("0.00");
                        }
                    }
                }
            }
        }
    }

}
