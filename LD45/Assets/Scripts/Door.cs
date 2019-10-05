using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private PowerHolder m_holder;

    bool m_closed = true;

    private Renderer m_renderer;
    private BoxCollider2D m_collider;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_holder)
        {
            SetClosed(m_holder.PowerLevel != m_holder.MaxPower);
        }
    }

    void SetClosed(bool closed)
    {
        if (closed != m_closed)
        {
            if(closed)
            {
                m_collider.enabled = true;
                m_renderer.enabled  = true;
            }
            else
            {
                m_collider.enabled = false;
                m_renderer.enabled = false;
            }

            m_closed = closed;
        }
    }
}
