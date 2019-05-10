using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image circle;
    public Image icon;
    public string title;
    public RadialMenu parent;
    public Action interaction;

    public TextMeshProUGUI hint;

    Color defaultColor;

    private void Start()
    {
        hint = GameObject.FindWithTag("hint").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        defaultColor = circle.color;
        circle.color = Color.green;
        parent.selected = this;
        hint.text = title;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        parent.selected = null;
        circle.color = defaultColor;
        hint.text = "";
    }
}
