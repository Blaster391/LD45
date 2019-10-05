using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHolder : MonoBehaviour
{
    private PowerHolder m_parentHolder;

    [SerializeField]
    private Sprite m_poweredSprite;
    [SerializeField]
    private Sprite m_unpoweredSprite;

    Powerball m_powerBall = null;
    SpriteRenderer m_renderer;

    public bool Powered { get { return m_powerBall != null; } }

    // Start is called before the first frame update
    void Start()
    {
        m_parentHolder = gameObject.GetComponentInParent<PowerHolder>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_powerBall)
        {
            m_renderer.sprite = m_poweredSprite;
        }
        else
        {
            m_renderer.sprite = m_unpoweredSprite;
        }
    }

    public bool TryToPower(Powerball _powerBall)
    {
        if(!m_powerBall)
        {
            if(_powerBall.Selected && _powerBall.Type == m_parentHolder.Type)
            {
                m_powerBall = _powerBall;
                m_powerBall.Power(this);

                return true;
            }
        }
        return false;
    }

    public void ReleasePowerball()
    {
        if (m_powerBall)
        {
            m_powerBall.Release();
            m_powerBall = null;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!m_powerBall)
        {
            Powerball powerball = col.gameObject.GetComponent<Powerball>();
            if(powerball)
            {
                TryToPower(powerball);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (m_powerBall)
        {
            if(m_powerBall.gameObject == col.gameObject)
            {
                ReleasePowerball();
            }
        }
    }
}
