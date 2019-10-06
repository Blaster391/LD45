using UnityEngine;

public class EndGameDoor : MonoBehaviour
{
    [SerializeField]
    private float m_openSpeed = 0.5f;
    [SerializeField]
    private float m_openAmount = 1.0f;

    [SerializeField]
    private GameObject m_bottomDoor;
    private Vector3 m_bottomDoorPosition;

    [SerializeField]
    private GameObject m_topDoor;
    private Vector3 m_topDoorPosition;

    [SerializeField]
    private Player m_player;

    bool m_doorOpen = false;

    bool m_playerPoximity = false;

    private float m_currentOpenAmount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_bottomDoorPosition = m_bottomDoor.gameObject.transform.position;
        m_topDoorPosition = m_topDoor.gameObject.transform.position;
    }

    
    void Update()
    {
        m_doorOpen = m_player.PowerPanel.CorePower.AtMaxPower && m_playerPoximity;
        if (m_doorOpen)
        {
            m_currentOpenAmount += Time.deltaTime *  m_openSpeed;
            if(m_currentOpenAmount > m_openAmount)
            {
                m_currentOpenAmount = m_openAmount;
            }
        }
        else
        {
            m_currentOpenAmount -= Time.deltaTime *  m_openSpeed;
            if (m_currentOpenAmount < 0)
            {
                m_currentOpenAmount = 0;
            }
        }

        m_bottomDoor.gameObject.transform.position = m_bottomDoorPosition + Vector3.down * m_currentOpenAmount;
        m_topDoor.gameObject.transform.position = m_topDoorPosition + Vector3.up * m_currentOpenAmount;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.GetComponent<Player>();
        if (p)
        {
            m_playerPoximity = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Player p = col.GetComponent<Player>();
        if (p)
        {
            m_playerPoximity = false;
        }
    }

}
