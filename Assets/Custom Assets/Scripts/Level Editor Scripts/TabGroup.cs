using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages tabs in a group.
/// </summary>
public class TabGroup : MonoBehaviour
{
    // The list of tabs in this group
    public List<ButtonTab> m_tabButtons;
    // Selected tab
    public ButtonTab m_selectedTab;

    public List<GameObject> m_objectsToSwap;

    // When a tab subscribes to this group, add it to the list
    public void Subscribe(ButtonTab _tabButton)
    {
        if (m_tabButtons == null)
        {
            m_tabButtons = new List<ButtonTab>();
        }
        m_tabButtons.Add(_tabButton);
    }

    public void OnTabEnter(ButtonTab _tabButton)
    {
       ResetTabs();
        if (m_selectedTab == null || _tabButton != m_selectedTab)
        {
            _tabButton.m_tabImage.color = new Color(205.0f / 255.0f, 205.0f / 255.0f, 205.0f / 255.0f, 1.0f);
        }
    }

    public void OnTabExit(ButtonTab _tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(ButtonTab _tabButton)
    {
        m_selectedTab = _tabButton;
        ResetTabs();
        _tabButton.m_tabImage.color = new Color(105.0f / 255.0f, 105.0f / 255.0f, 105.0f / 255.0f, 1.0f);

        int index = _tabButton.transform.GetSiblingIndex();

        for (int i = 0; i < m_objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                m_objectsToSwap[i].SetActive(true);
            }
            else
            {
                m_objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (ButtonTab _button in m_tabButtons)
        {
            // Skip through the selected tab
            if (m_selectedTab != null && _button == m_selectedTab)
            {
                continue;
            }
            _button.m_tabImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
