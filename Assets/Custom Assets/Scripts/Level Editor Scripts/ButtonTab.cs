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
    public TabGroup tabGroup;
    // Image attached to this tab
    public Image tabImage;

    private Button button;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(button.interactable)
            tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
            tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
            tabGroup.OnTabExit(this);
    }

    private void Start()
    {
        tabImage = GetComponent<Image>();
        tabGroup.Subscribe(this);

        button = GetComponent<Button>();
    }
}
