using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPanel : MonoBehaviour
{
    [SerializeField]
    private PowerHolder m_corePower;
    public PowerHolder CorePower { get { return m_corePower; } }

    [SerializeField]
    private PowerHolder m_movementPower;
    public PowerHolder MovementPower { get { return m_movementPower; } }

    [SerializeField]
    private PowerHolder m_jumpPower;
    public PowerHolder JumpPower { get { return m_jumpPower; } }

    [SerializeField]
    private GameObject m_coreTether;
    public GameObject CoreTether { get { return m_coreTether; } }

    [SerializeField]
    private GameObject m_abilityTether;
    public GameObject AbilityTether { get { return m_abilityTether; } }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
