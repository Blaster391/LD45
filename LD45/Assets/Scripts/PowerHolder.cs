using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerHolder : MonoBehaviour
{
    [SerializeField]
    private int m_maxPower = 1;


    [SerializeField]
    private List<BallHolder> m_holders;

    [SerializeField]
    private PowerType m_type;

    public PowerType Type { get { return m_type; } }

    public int PowerLevel { get { return m_holders.Count(x => x.Powered); } }
    public int MaxPower => m_maxPower;
    public bool AtMaxPower { get { return PowerLevel == MaxPower; } }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < m_holders.Count; ++i)
        {
            m_holders[i].gameObject.SetActive(i < m_maxPower);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
