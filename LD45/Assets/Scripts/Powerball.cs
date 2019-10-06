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
    Player m_player;

    [SerializeField]
    private PowerType m_type;
    public PowerType Type => m_type;
    public BallState State => m_state;

    private BallHolder m_holder;
    private LineRenderer m_lineRenderer;

    [SerializeField]
    BallState m_state;

    [SerializeField]
    float m_moveSpeed = 5.0f;

    [SerializeField]
    float m_orbitSpeedRand = 1.0f;
    [SerializeField]
    float m_orbitSpeed = 1.0f;

    [SerializeField]
    float m_bobDistance = 1.0f;
    [SerializeField]
    float m_bobSpeed = 0.25f;

    [SerializeField]
    float m_orbitDistanceRand = 1.0f;
    [SerializeField]
    float m_orbitDistance = 1.0f;

    float m_currentOrbit = 0.0f;
    float m_currentBob = 0.0f;
    Vector3 m_startingPosition;

    [SerializeField]
    float m_rotateSpeed = 1.0f;

    bool m_selected = false;
    public bool Selected { get { return m_selected; } }

    Vector3 startingScale = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        m_startingPosition = gameObject.transform.position;
        startingScale = gameObject.transform.localScale;
        m_currentOrbit = Random.value * Mathf.PI * 2;
        m_currentBob = Random.value * Mathf.PI * 2;

        m_orbitSpeed += m_orbitSpeedRand * Random.value;
        m_orbitDistance += m_orbitDistanceRand * Random.value;

        Camera.main.gameObject.GetComponent<ScreenFX>().AddPointFX(gameObject);

        m_lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //This is dumb
        Transform ogParent = gameObject.transform.parent;
        gameObject.transform.parent = null;
        gameObject.transform.localScale = startingScale;
        gameObject.transform.parent = ogParent;

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
                Vector3 targetPosition = m_player.gameObject.transform.position;
                gameObject.transform.parent = m_player.gameObject.transform;
                if (m_type == PowerType.Core)
                {
                    targetPosition = m_player.PowerPanel.CoreTether.transform.position;
                    gameObject.transform.parent = m_player.PowerPanel.CoreTether.transform;


                }
                else if(m_type == PowerType.Ability)
                {
                    targetPosition = m_player.PowerPanel.AbilityTether.transform.position;
                    gameObject.transform.parent = m_player.PowerPanel.AbilityTether.transform;
                }

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

        //if (m_state != BallState.InUse)
        //{
            transform.Rotate(new Vector3(0, 0, m_rotateSpeed * Time.deltaTime));
        //}

        if (m_state == BallState.Held && gameObject.transform.parent)
        {
            m_lineRenderer.enabled = true;
            Vector3 lineFrom = gameObject.transform.position;
            lineFrom.z = 100;
            Vector3 lineTo = gameObject.transform.parent.transform.position;
            lineTo.z = 100;

            Vector3[] positions = { lineFrom, lineTo };
            m_lineRenderer.SetPositions(positions);
        }
        else
        {
            m_lineRenderer.enabled = false;
            Vector3[] positions = { };
            m_lineRenderer.SetPositions(positions);
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

            if (m_type == PowerType.Core)
            {
                gameObject.transform.parent = m_player.PowerPanel.CoreTether.transform;
            }
            else if (m_type == PowerType.Ability)
            {
                gameObject.transform.parent = m_player.PowerPanel.AbilityTether.transform;
            }
            else
            {
                gameObject.transform.parent = m_player.gameObject.transform;
            }
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
                m_player = col.gameObject.GetComponent<Player>();
                m_state = BallState.Held;
            }
        }

    }
}
