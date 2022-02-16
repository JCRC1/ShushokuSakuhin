using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Tab button manager.
/// </summary>
[RequireComponent(typeof(Image))]
public class ButtonTab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    // Reference to tab group this tab belongs to
    public TabGroup m_tabGroup;
    // Image attached to this tab
    public Image m_tabImage;

    private Button m_button;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(m_button.interactable)
            m_tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_button.interactable)
            m_tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_button.interactable)
            m_tabGroup.OnTabExit(this);
    }

    private void Start()
    {
        m_tabImage = GetComponent<Image>();
        m_tabGroup.Subscribe(this);

        m_button = GetComponent<Button>();
    }
}
