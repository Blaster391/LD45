using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class EndGameTime : MonoBehaviour
{
    // Start is called before the first frame update
    int time = 120;

    Text m_text;

    void Start()
    {
       GameObject timer = GameObject.Find("Timer");
        if(timer)
        {
            time = Mathf.RoundToInt(timer.GetComponent<Timer>().m_time);
        }
        Destroy(timer);
        m_text = GetComponent<Text>();
        m_text.text = $"Submitted in {time} seconds";

        Analytics.CustomEvent("Completed", new Dictionary<string, object>
        {
            { "time", time }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
