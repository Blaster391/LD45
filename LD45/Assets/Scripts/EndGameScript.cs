using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    [SerializeField]
    float m_rotateSpeed = 0.0f;

    [SerializeField]
    float m_colourChangeRate = 1.0f;
    [SerializeField]
    List<Color> m_colours;

    float m_timeSinceColourChange = 0.0f;
    Color m_currentColour;
    Color m_nextColour;

    SpriteRenderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_currentColour = RandomColour();
        m_nextColour = RandomColour();

        m_renderer = gameObject.GetComponent<SpriteRenderer>();

        Camera.main.gameObject.GetComponent<ScreenFX>().AddPointFX(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        m_timeSinceColourChange += Time.deltaTime;
        Color c = Color.Lerp(m_currentColour, m_nextColour, m_timeSinceColourChange / m_colourChangeRate);
        c.a = 1;
        m_renderer.material.color = c;

        if (m_timeSinceColourChange > m_colourChangeRate)
        {
            m_timeSinceColourChange = 0;
            m_currentColour = m_nextColour;
            m_nextColour = RandomColour();
        }

        transform.Rotate(new Vector3(0, 0, m_rotateSpeed * Time.deltaTime));
    }

    Color RandomColour()
    {
        if(m_colours.Count == 0)
        {
            return Color.white;
        }
        else
        {
            int randomIndex = Random.Range(0, m_colours.Count);
            return m_colours[randomIndex];
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.GetComponent<Player>();
        if (p)
        {
            Camera.main.gameObject.GetComponent<ScreenFX>().Finished();
        }
    }
}
