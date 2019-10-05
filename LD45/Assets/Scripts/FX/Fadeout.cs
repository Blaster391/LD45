using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadeout : MonoBehaviour
{
    [SerializeField]
    float m_maxAge = 1.0f;
    float m_age = 0;

    Renderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        var colour = m_renderer.material.color;
        colour.a = 0.5f;
        m_renderer.material.color = colour;
    }

    // Update is called once per frame
    void Update()
    {
        m_age += Time.deltaTime;

        var colour = m_renderer.material.color;
        colour.a = 1.0f - m_age / m_maxAge;
        m_renderer.material.color = colour;

        if (m_age > m_maxAge)
        {
            Destroy(gameObject);
        }
    }
}
