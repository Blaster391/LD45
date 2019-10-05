using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private Sprite m_active;
    [SerializeField]
    private Sprite m_inactive;

    SpriteRenderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetActive()
    {
        m_renderer.sprite = m_active;
    }

    public void SetInactive()
    {
        m_renderer.sprite = m_inactive;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.GetComponent<Player>();
        if(p)
        {
            p.SetCheckpoint(this);
        }
    }
}
