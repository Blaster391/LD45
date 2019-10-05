using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHolder : MonoBehaviour
{
    private PowerHolder m_parentHolder;

    // Start is called before the first frame update
    void Start()
    {
        m_parentHolder = gameObject.GetComponentInParent<PowerHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
