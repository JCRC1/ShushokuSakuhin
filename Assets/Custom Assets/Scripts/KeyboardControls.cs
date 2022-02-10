using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
    public static KeyboardControls Instance;


    /// <summary>
    /// The Red Lanes
    /// </summary>
    public KeyCode[] m_oddLaneKeybind;

    /// <summary>
    /// The Blue Lanes
    /// </summary>
    public KeyCode[] m_evenLaneKeybind;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Even
        for (int i = 0; i < m_evenLaneKeybind.Length; i++)
        {
            if (!Input.GetKeyDown(m_evenLaneKeybind[i]))
            {
                continue;
            }

            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_identifier % 2 == 0)
                {
                    continue;
                }

                lane.GetComponent<LineRenderer>().material.SetColor("_Color", lane.GetComponent<LineRenderer>().material.color * Mathf.Pow(2, 1));

                if (lane.m_notes.Count <= 0)
                {
                    continue;
                }

                switch (lane.m_notes.Peek().m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        Debug.Log("HIT PERFECT EVEN");
                        ScoreController.Instance.AddPerfectHit();
                        lane.m_notes.Peek().gameObject.SetActive(false);
                        lane.m_notes.Dequeue();
                        break;
                    case NoteHandler.NoteState.GOOD:
                        Debug.Log("HIT GOOD EVEN");
                        ScoreController.Instance.AddGoodHit();
                        lane.m_notes.Peek().gameObject.SetActive(false);
                        lane.m_notes.Dequeue();
                        break;
                    case NoteHandler.NoteState.MISS:
                        break;
                    default:
                        break;
                }
            }            
        }

        // Odd
        for (int i = 0; i < m_oddLaneKeybind.Length; i++)
        {
            if (!Input.GetKeyDown(m_oddLaneKeybind[i]))
            {
                continue;
            }

            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_identifier % 2 != 0)
                {
                    continue;
                }

                lane.GetComponent<LineRenderer>().material.SetColor("_Color", lane.GetComponent<LineRenderer>().material.color * Mathf.Pow(2, 1));

                if (lane.m_notes.Count <= 0)
                {
                    continue;
                }

                switch (lane.m_notes.Peek().m_noteState)
                {
                    case NoteHandler.NoteState.NONE:
                        break;
                    case NoteHandler.NoteState.PERFECT:
                        Debug.Log("HIT PERFECT EVEN");
                        ScoreController.Instance.AddPerfectHit();
                        lane.m_notes.Peek().gameObject.SetActive(false);
                        lane.m_notes.Dequeue();
                        break;
                    case NoteHandler.NoteState.GOOD:
                        Debug.Log("HIT GOOD EVEN");
                        ScoreController.Instance.AddGoodHit();
                        lane.m_notes.Peek().gameObject.SetActive(false);
                        lane.m_notes.Dequeue();
                        break;
                    case NoteHandler.NoteState.MISS:
                        break;
                    default:
                        break;
                }
            }
        }


        for (int i = 0; i < m_evenLaneKeybind.Length; i++)
        {
            if (!Input.GetKeyUp(m_evenLaneKeybind[i]))
            {
                continue;
            }

            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_identifier % 2 == 0)
                {
                    continue;
                }

                lane.GetComponent<LineRenderer>().material.SetColor("_Color", lane.GetComponent<LineRenderer>().material.color / Mathf.Pow(2, 1));
            }
        }


        for (int i = 0; i < m_oddLaneKeybind.Length; i++)
        {
            if (!Input.GetKeyUp(m_oddLaneKeybind[i]))
            {
                continue;
            }

            for (int j = 0; j < GameManager.Instance.m_lanes.Count; j++)
            {
                LaneHandler lane = GameManager.Instance.m_lanes[j].GetComponent<LaneHandler>();

                if (lane.m_identifier % 2 != 0)
                {
                    continue;
                }

                lane.GetComponent<LineRenderer>().material.SetColor("_Color", lane.GetComponent<LineRenderer>().material.color / Mathf.Pow(2, 1));
            }
        }
    }
}
