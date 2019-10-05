using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_player;
    [SerializeField]
    private float m_offset = 5;
    [SerializeField]
    private float m_velocityAdjustScale = 0.1f;
    [SerializeField]
    private float m_velocityAdjustCap = 2.0f;

    private float m_depth;

    void Start()
    {
        m_depth = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = m_player.transform.position;
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
    }
}
