using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PowerPanel m_powerPanel;

    [SerializeField]
    private float m_maxMoveSpeed = 5;
    [SerializeField]
    private float m_movementForce = 5;
    [SerializeField]
    private float m_jumpForce = 10;
    [SerializeField]
    private float m_doubleJumpForce = 10;
    [SerializeField]
    private float m_dashForce = 20;

    private BoxCollider2D m_collider;
    private Rigidbody2D m_rigidbody;
    private float m_groundedThreshold = 0.1f;


    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();

    }

    private void ProcessMovement()
    {
        Vector2 movementForce = new Vector2();
        if (m_powerPanel.MovementPower.PowerLevel > 0)
        {
            if (m_rigidbody.velocity.magnitude < m_maxMoveSpeed)
            {
                movementForce += Vector2.right * Input.GetAxis("Horizontal") * m_movementForce;
            }
        }
        m_rigidbody.AddForce(movementForce, ForceMode2D.Force);

        if (m_powerPanel.JumpPower.PowerLevel > 0)
        {
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                Vector2 jumpForce = Vector2.up * m_jumpForce;
                m_rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
            }
        }


    }

    private bool IsGrounded()
    {
        List<RaycastHit2D> results = new List<RaycastHit2D>();
        ContactFilter2D filter = new ContactFilter2D();
        Physics2D.BoxCast(m_rigidbody.position, m_collider.size, 0, Vector2.down, filter, results, m_groundedThreshold);
        foreach(var result in results)
        {
            if(result.collider.gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }
 }
