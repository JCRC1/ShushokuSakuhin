using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewCoverArtPath : MonoBehaviour
{
    private Text path;

    private void Start()
    {
        path = GetComponent<Text>();
    }

    private void Update()
    {
        if (ChartMetadataBuilder.chartData != null)
            path.text = ChartMetadataBuilder.chartData.trackCoverArtPath;
    }
}
