using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LaneEditor : MonoBehaviour
{
    // Singleton declaration
    public static LaneEditor Instance;

    public Camera m_worldCam;
    private Ray m_ray;
    private RaycastHit m_hit;

    //[HideInInspector]
    public LaneHandler m_selectedLane;

    public Color m_highlightedCol;
    public Color m_defaultCol;

    public InputField m_startPosX;
    public InputField m_startPosY;

    public TempLaneInit m_tempLaneInit;

    public GameObject m_dummyLanePrefab;

    private GameObject m_dummyLane;

    private void Awake()
    {
        // Singleton declaration
        Instance = this;
    }

    private void Start()
    {
        m_dummyLane = Instantiate(m_dummyLanePrefab);
        Destroy(m_dummyLane.GetComponent<DummyLaneMovement>());
        m_dummyLane.SetActive(false);
    }

    private void Update()
    {
        var view = m_worldCam.ScreenToViewportPoint(Input.mousePosition);
        var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

        // Create a ray from the screen down to the world
        m_ray = m_worldCam.ScreenPointToRay(Input.mousePosition);

        if (!isOutside)
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 vec2;
                    vec2 = m_worldCam.ScreenToWorldPoint(Input.mousePosition);
                    vec2.x = Mathf.Round(vec2.x);
                    vec2.y = Mathf.Round(vec2.y);

                    m_startPosX.text = vec2.x.ToString();
                    m_startPosY.text = vec2.y.ToString();

                    m_tempLaneInit.m_tempLaneData.m_initialPosition = vec2;
                    m_dummyLane.SetActive(true);
                    m_dummyLane.transform.position = vec2;
                }
            }
            else
            {
                m_dummyLane.SetActive(false);
            }
            // Check if there isnt already a lane selected
            if (!m_selectedLane)
            {
                // If we hit something, in this case, the lane, and click...
                if (Physics.Raycast(m_ray, out m_hit))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (m_hit.collider.tag == "Lane")
                        {
                            // It is now selected
                            m_selectedLane = m_hit.collider.GetComponent<LaneHandler>();
                            m_defaultCol = m_selectedLane.GetComponent<LineRenderer>().startColor;
                        }
                    }
                }
            }
            else
            {
                if (Physics.Raycast(m_ray, out m_hit))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (m_hit.collider.tag != "MGizmo" && m_hit.collider.tag != "Dummy")
                        {
                            m_selectedLane.GetComponent<LineRenderer>().startColor = m_defaultCol;
                            m_selectedLane.GetComponent<LineRenderer>().endColor = m_defaultCol;
                            m_selectedLane = null;
                        }
                    }
                }
            }


            if (Input.mouseScrollDelta.y > 0)
            {
                m_worldCam.orthographicSize--;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                m_worldCam.orthographicSize++;
            }

            if (m_worldCam.orthographicSize <= 10)
            {
                m_worldCam.orthographicSize = 10;
            }
            if (m_worldCam.orthographicSize >= 50)
            {
                m_worldCam.orthographicSize = 50;
            }
        }

        if (m_selectedLane)
        {
            m_selectedLane.GetComponent<LineRenderer>().startColor = m_highlightedCol;
            m_selectedLane.GetComponent<LineRenderer>().endColor = m_highlightedCol;
        }
    }

    public void SetSelectedLane(Text _text)
    {
        if (m_selectedLane)
        {
            m_selectedLane.GetComponent<LineRenderer>().startColor = m_defaultCol;
            m_selectedLane.GetComponent<LineRenderer>().endColor = m_defaultCol;
        }

        m_selectedLane = LevelEditorManager.Instance.m_lanes[int.Parse(_text.text)].GetComponent<LaneHandler>();
    }
}
