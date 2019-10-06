using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    PowerPanel m_powerPanel;
    [SerializeField]
    private GameObject m_portal;

    [SerializeField]
    private GameObject m_player;
    [SerializeField]
    private float m_offset = 5;
    [SerializeField]
    private float m_velocityAdjustScale = 0.1f;
    [SerializeField]
    private float m_velocityAdjustCap = 2.0f;

    private float m_depth;

    ScreenFX m_fx;

    void Start()
    {
        m_depth = gameObject.transform.position.z;
        m_fx = GetComponent<ScreenFX>();
    }

    // Update is called once per frame

    float m_endGameMod = 0.0f;

    Vector3 m_previousPosition = new Vector3();

    void Update()
    {
        Vector3 position = m_player.transform.position;
        if(m_fx.EndGame)
        {
            m_endGameMod += Time.deltaTime * 0.2f;
        }
        else
        {
            m_endGameMod -= Time.deltaTime * 0.5f;
        }
        m_endGameMod = Mathf.Clamp01(m_endGameMod);

        Vector3 midPos = position + (m_portal.transform.position - Vector3.up * m_offset - position) * 0.5f;

        position = Vector3.Lerp(position, midPos, m_endGameMod);


        position.z = m_depth;
        position += Vector3.up * m_offset;

        Vector3 velocityAdjust = m_player.GetComponent<Rigidbody2D>().velocity * m_velocityAdjustScale;
        if(velocityAdjust.magnitude > m_velocityAdjustCap)
        {
            velocityAdjust.Normalize();
            velocityAdjust *= m_velocityAdjustCap;
        }
        position += velocityAdjust;

        gameObject.transform.position = position;

        SortOutPowerPanel();

        m_previousPosition = position;
    }

    void SortOutPowerPanel()
    {
        Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 10));

        stageDimensions.x -= 1.5f;

        m_powerPanel.gameObject.transform.position = stageDimensions;


        Vector3 localPos = m_powerPanel.gameObject.transform.localPosition;
        localPos.y = 0.0f;
        m_powerPanel.gameObject.transform.localPosition = localPos;
    }
}
