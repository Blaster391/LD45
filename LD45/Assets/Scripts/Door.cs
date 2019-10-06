using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private PowerHolder m_holder;

    [SerializeField]
    private float m_fadeSpeed = 1.0f;

    private float m_currentFade = 0.0f;

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

        if(m_closed)
        {
            m_currentFade += m_fadeSpeed * Time.deltaTime;
        }
        else
        {
            m_currentFade -= m_fadeSpeed * Time.deltaTime;
        }
        m_currentFade = Mathf.Clamp01(m_currentFade);

        var color = m_renderer.material.color;
        color.a = m_currentFade;
        m_renderer.material.color = color;

        if(m_currentFade <= 0.0f)
        {
            m_collider.enabled = false;
            m_renderer.enabled = false;
        }
        else
        {
            m_collider.enabled = true;
            m_renderer.enabled = true;
        }

        if(m_closed && m_player)
        {
            m_player.Respawn();
        }
    }

    void SetClosed(bool closed)
    {
        if (closed != m_closed)
        {
            Audio.AUDIO.PlayClip(Audio.AUDIO.m_door);
            m_closed = closed;
        }


    }

    Player m_player = null;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.isTrigger)
        {
            return;
        }

        Player p = col.GetComponent<Player>();
        if (p)
        {
            m_player = p;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.isTrigger)
        {
            return;
        }
        Player p = col.GetComponent<Player>();
        if (p)
        {
            m_player = null;
        }
    }
}
