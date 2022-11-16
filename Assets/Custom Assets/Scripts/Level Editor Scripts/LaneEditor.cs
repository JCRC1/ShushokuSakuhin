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

    public Camera worldCam;
    private Ray ray;
    private RaycastHit hit;

    //[HideInInspector]
    public LaneHandler selectedLane;

    public Color highlightedCol;
    public Color defaultCol;

    public InputField startPosX;
    public InputField startPosY;

    public TempLaneInit tempLaneInit;

    public GameObject dummyLanePrefab;

    private GameObject dummyLane;

    private void Awake()
    {
        // Singleton declaration
        Instance = this;
    }

    private void Start()
    {
        dummyLane = Instantiate(dummyLanePrefab);
        Destroy(dummyLane.GetComponent<DummyLaneMovement>());
        dummyLane.SetActive(false);
    }

    private void Update()
    {
        var view = worldCam.ScreenToViewportPoint(Input.mousePosition);
        var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

        // Create a ray from the screen down to the world
        ray = worldCam.ScreenPointToRay(Input.mousePosition);

        if (selectedLane)
        {
            if (selectedLane.identifier % 2 == 0)
                defaultCol = Color.blue;
            else
                defaultCol = Color.red;
        }

        if (!isOutside)
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 vec2;
                    vec2 = worldCam.ScreenToWorldPoint(Input.mousePosition);
                    vec2.x = Mathf.Round(vec2.x);
                    vec2.y = Mathf.Round(vec2.y);

                    startPosX.text = vec2.x.ToString();
                    startPosY.text = vec2.y.ToString();

                    tempLaneInit.tempLaneData.initialPosition = vec2;
                    dummyLane.SetActive(true);
                    dummyLane.transform.position = vec2;
                }
            }
            else
            {
                dummyLane.SetActive(false);
            }
            // Check if there isnt already a lane selected
            if (!selectedLane)
            {
                // If we hit something, in this case, the lane, and click...
                if (Physics.Raycast(ray, out hit))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (hit.collider.tag == "Lane")
                        {
                            // It is now selected
                            selectedLane = hit.collider.GetComponent<LaneHandler>();
                            defaultCol = selectedLane.GetComponent<LineRenderer>().startColor;

                            for (int i = 0; i < SelectedLaneDisplay.Instance.indexDisplay.Length; i++)
                            {
                                SelectedLaneDisplay.Instance.indexDisplay[i].text = selectedLane.identifier.ToString();
                            }
                        }
                    }
                }
            }
            else
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (hit.collider.tag != "MGizmo" && hit.collider.tag != "Dummy")
                        {
                            selectedLane.GetComponent<LineRenderer>().startColor = defaultCol;
                            selectedLane.GetComponent<LineRenderer>().endColor = defaultCol;

                            for (int i = 0; i < SelectedLaneDisplay.Instance.indexDisplay.Length; i++)
                            {
                                SelectedLaneDisplay.Instance.indexDisplay[i].text = "None";
                            }
                            selectedLane = null;
                        }
                    }
                }
            }


            if (Input.mouseScrollDelta.y > 0)
            {
                worldCam.orthographicSize--;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                worldCam.orthographicSize++;
            }

            if (worldCam.orthographicSize <= 10)
            {
                worldCam.orthographicSize = 10;
            }
            if (worldCam.orthographicSize >= 50)
            {
                worldCam.orthographicSize = 50;
            }
        }

        if (selectedLane)
        {
            selectedLane.GetComponent<LineRenderer>().startColor = highlightedCol;
            selectedLane.GetComponent<LineRenderer>().endColor = highlightedCol;
        }
    }

    public void SetSelectedLane(Text _text)
    {
        if (selectedLane)
        {
            selectedLane.GetComponent<LineRenderer>().startColor = defaultCol;
            selectedLane.GetComponent<LineRenderer>().endColor = defaultCol;
        }

        selectedLane = LevelEditorManager.Instance.lanes[int.Parse(_text.text)].GetComponent<LaneHandler>();
    }
}
