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

    [SerializeField]
    private float m_respawnTime = 3.0f;
    [SerializeField]
    private Checkpoint m_checkpoint;
    public Checkpoint Checkpoint => m_checkpoint;

    bool m_respawning = false;
    float m_timeRespawning = 0.0f;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();

        if (m_checkpoint)
        {
            m_checkpoint.SetActive();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_respawning)
        {
            ProcessRespawn();
        }
        else
        {
            ProcessMovement();
        }

        if(transform.position.y < -25.0f)
        {
            Respawn();
        }
    }

    public void SetCheckpoint(Checkpoint _checkpoint)
    {
        if (m_checkpoint)
        {
            m_checkpoint.SetInactive();
        }

        m_checkpoint = _checkpoint;
        m_checkpoint.SetActive();
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

    public void Respawn()
    {
        if(!m_respawning)
        {
            m_respawning = true;
            m_timeRespawning = 0.0f;
        }
    }
    public void ProcessRespawn()
    {
        //TODO fade
        m_timeRespawning += Time.deltaTime;
        if (m_timeRespawning < m_respawnTime * 0.5f)
        {
            
        }
        else if(m_timeRespawning < m_respawnTime)
        {
            gameObject.transform.position = m_checkpoint.transform.position;
        }
        else
        {
            m_respawning = false;
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
