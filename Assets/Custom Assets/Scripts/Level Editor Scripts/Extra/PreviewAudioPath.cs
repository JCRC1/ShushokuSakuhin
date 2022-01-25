using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewAudioPath : MonoBehaviour
{
    private Text m_path;

    private void Start()
    {
        m_path = GetComponent<Text>();
    }

    private void Update()
    {
        if(ChartMetadataBuilder.m_chartData != null)
        m_path.text = ChartMetadataBuilder.m_chartData.m_trackAudioPath;
    }
}
