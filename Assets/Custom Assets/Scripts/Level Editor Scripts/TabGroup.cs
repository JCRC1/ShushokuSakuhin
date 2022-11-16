using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages tabs in a group.
/// </summary>
public class TabGroup : MonoBehaviour
{
    // The list of tabs in this group
    public List<ButtonTab> tabButtons;
    // Selected tab
    public ButtonTab selectedTab;

    public List<GameObject> objectsToSwap;

    // When a tab subscribes to this group, add it to the list
    public void Subscribe(ButtonTab _tabButton)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<ButtonTab>();
        }
        tabButtons.Add(_tabButton);
    }

    public void OnTabEnter(ButtonTab _tabButton)
    {
       ResetTabs();
        if (selectedTab == null || _tabButton != selectedTab)
        {
            _tabButton.tabImage.color = new Color(205.0f / 255.0f, 205.0f / 255.0f, 205.0f / 255.0f, 1.0f);
        }
    }

    public void OnTabExit(ButtonTab _tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(ButtonTab _tabButton)
    {
        selectedTab = _tabButton;
        ResetTabs();
        _tabButton.tabImage.color = new Color(105.0f / 255.0f, 105.0f / 255.0f, 105.0f / 255.0f, 1.0f);

        int index = _tabButton.transform.GetSiblingIndex();

        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (ButtonTab _button in tabButtons)
        {
            // Skip through the selected tab
            if (selectedTab != null && _button == selectedTab)
            {
                continue;
            }
            _button.tabImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
