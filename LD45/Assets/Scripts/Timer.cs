using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float m_time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_time = 0.0f;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
    }
}
