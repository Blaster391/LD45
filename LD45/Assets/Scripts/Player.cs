using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PowerPanel m_powerPanel;

    [SerializeField]
    private float m_movementForce;
    [SerializeField]
    private float m_jumpForce;
    [SerializeField]
    private float m_doubleJumpForce;
    [SerializeField]
    private float m_dashForce;

    private Rigidbody2D m_rigidbody;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();

    }

    private void ProcessMovement()
    {
        Vector2 movementForce = new Vector2();
        bool useImpulse = false;

        if (m_powerPanel.MovementPower.PowerLevel > 1)
        {
            if (Input.GetButton("Left"))
            {
                movementForce += Vector2.left * m_movementForce;
            }

            if (Input.GetButton("Right"))
            {
                movementForce += Vector2.right * m_movementForce;
            }
        }

        if (m_powerPanel.JumpPower.PowerLevel > 1)
        {
            if (Input.GetButtonDown("Jump"))
            {
                movementForce += Vector2.up * m_jumpForce;
                useImpulse = true;
            }
        }
        ForceMode2D forceMode = useImpulse ? ForceMode2D.Impulse : ForceMode2D.Force;
        m_rigidbody.AddForce(movementForce, forceMode);
    }
}
