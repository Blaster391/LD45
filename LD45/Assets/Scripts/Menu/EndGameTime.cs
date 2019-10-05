using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    int time = 120;

    Text m_text;

    [SerializeField]
    Color m_regularColour = Color.white;

    [SerializeField]
    Color m_hoverColor = Color.grey;

    void Start()
    {
        m_text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_text.color = m_hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_text.color = m_regularColour;
    }
}
