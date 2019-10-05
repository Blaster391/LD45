using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerType
{
    Core,
    Ability,
    External
}

public enum BallState
{
    Free,
    InUse,
    Held
}

public class Powerball : MonoBehaviour
{

    [SerializeField]
    GameObject m_player;

    [SerializeField]
    private PowerType m_type;
    public PowerType Type => m_type;
    public BallState State => m_state;

    private BallHolder m_holder;

    [SerializeField]
    BallState m_state;

    [SerializeField]
    float m_moveSpeed = 5.0f;

    [SerializeField]
    float m_orbitSpeed = 1.0f;

    [SerializeField]
    float m_bobDistance = 1.0f;
    [SerializeField]
    float m_bobSpeed = 0.25f;

    [SerializeField]
    float m_orbitDistance = 1.0f;

    float m_currentOrbit = 0.0f;
    float m_currentBob = 0.0f;
    Vector3 m_startingPosition;

    bool m_selected = false;
    public bool Selected { get { return m_selected; } }

    // Start is called before the first frame update
    void Start()
    {
        m_startingPosition = gameObject.transform.position;
        m_currentOrbit = Random.value * Mathf.PI * 2;
        m_currentBob = Random.value * Mathf.PI * 2;

        Camera.main.gameObject.GetComponent<ScreenFX>().AddPointFX(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        if (m_selected)
        {
            if(Input.GetMouseButtonUp(0))
            {
                m_selected = false;
                Debug.Log("Released");
            }
            else
            {
                var cursorPos = Input.mousePosition;
                cursorPos.z = 10;
                cursorPos = Camera.main.ScreenToWorldPoint(cursorPos);
                gameObject.transform.position = cursorPos;
            }
        }
        else
        {
            if (m_state == BallState.Held && !m_selected)
            {
                Vector3 targetPosition = m_player.transform.position;

                targetPosition += new Vector3(Mathf.Sin(m_currentOrbit), Mathf.Cos(m_currentOrbit), 0) * m_orbitDistance;

                Vector3 travel = targetPosition - gameObject.transform.position;
                if (travel.magnitude > m_moveSpeed * Time.deltaTime)
                {
                    travel.Normalize();
                    travel *= m_moveSpeed * Time.deltaTime;
                }

                gameObject.transform.position += travel;
                m_currentOrbit += m_orbitSpeed * Time.deltaTime;

            }
            else if (m_state == BallState.InUse)
            {
                gameObject.transform.localPosition = new Vector3(0,0, -1);
            }
            else if(m_state == BallState.Free)
            {
                gameObject.transform.position = m_startingPosition + (Vector3.up * Mathf.Sin(m_currentBob) * m_bobDistance);
                m_currentBob += Time.deltaTime * m_bobSpeed;
            }
        }
    }

    void OnMouseOver()
    {
        if (m_state != BallState.Free)
        {
        
        }
    }

    void OnMouseDown()
    {
        if(m_state != BallState.Free)
        {
            Debug.Log("HIT");
            m_selected = true;

        }

    }

    public void Release()
    {
        if(m_holder)
        {
            m_holder = null;
            gameObject.transform.SetParent(null);
        }

        if (m_state == BallState.InUse)
        {
            m_state = BallState.Held;
        }

    }

    public void Power(BallHolder ballHolder)
    {
        if(m_state == BallState.Held)
        {
            m_holder = ballHolder;
            m_state = BallState.InUse;
            gameObject.transform.SetParent(m_holder.transform);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (m_state == BallState.Free)
        {
            if (col.gameObject.tag == "Player")
            {
                m_player = col.gameObject;
                m_state = BallState.Held;
            }
        }

    }
}
