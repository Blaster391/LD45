using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PowerPanel m_powerPanel;
    public PowerPanel PowerPanel => m_powerPanel;

    [SerializeField]
    public GameObject m_fadeout;

    [SerializeField]
    private float m_maxHorizontalMoveSpeed = 5;
    [SerializeField]
    private float m_maxSpeed = 5;
    [SerializeField]
    private float m_movementForce = 5;
    [SerializeField]
    private float m_jumpForce = 10;
    [SerializeField]
    private float m_doubleJumpForce = 10;
    [SerializeField]
    private float m_dashCooldown = 3;
    [SerializeField]
    private float m_dashForce = 20;
    [SerializeField]
    private float m_dashGhostRate = 0.1f;
    [SerializeField]
    private float m_dashGhostTime = 1.0f;

    private bool m_dashing = false;
    private float m_timeSinceDashed = 0.0f;
    private float m_timeSinceSpawnedDashGhost = 0.0f;

    private Renderer m_renderer;
    private BoxCollider2D m_collider;
    private Rigidbody2D m_rigidbody;
    private float m_groundedThreshold = 0.1f;

    [SerializeField]
    private float m_respawnTime = 3.0f;
    [SerializeField]
    private Checkpoint m_checkpoint;
    public Checkpoint Checkpoint => m_checkpoint;

    bool m_hasDoubleJumped = false;
    bool m_respawning = false;
    float m_timeRespawning = 0.0f;
    float m_startingGravity = 0.0f;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();

        if (m_checkpoint)
        {
            m_checkpoint.SetActive();
        }
        m_startingGravity = m_rigidbody.gravityScale;
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
            ProcessSelection();
        }

        if(transform.position.y < -25.0f)
        {
            Respawn();
        }
    }

    public void ProcessSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var cursorPos = Input.mousePosition;
            cursorPos.z = 0;
            cursorPos = Camera.main.ScreenToWorldPoint(cursorPos);

            RaycastHit2D[] hits = Physics2D.RaycastAll(cursorPos, Vector3.zero, 100);
            foreach(var hit in hits)
            {
                Powerball p = hit.collider.gameObject.GetComponent<Powerball>();
                if (p && p.State != BallState.Free)
                {
                    p.Select();
                    return;
                }
            }
        }
    }

    public void SetCheckpoint(Checkpoint _checkpoint)
    {
        if(m_checkpoint == _checkpoint)
        {
            return;
        }

        if (m_checkpoint)
        {
            m_checkpoint.SetInactive();
        }

        Audio.AUDIO.PlayClip(Audio.AUDIO.m_checkpoint);

        m_checkpoint = _checkpoint;
        m_checkpoint.SetActive();
    }

    private void ProcessMovement()
    {
        bool moved = false;
        bool grounded = IsGrounded();

        if (m_dashing)
        {
            
            m_timeSinceSpawnedDashGhost += Time.deltaTime;

            if(m_timeSinceSpawnedDashGhost > m_dashGhostRate)
            {
                SpawnFadeout();
                m_timeSinceSpawnedDashGhost = 0.0f;
            }

            if(m_timeSinceDashed > m_dashGhostTime)
            {
                m_dashing = false;
            }
        }
        m_timeSinceDashed += Time.deltaTime;


        if (m_powerPanel.MovementPower.PowerLevel > 0)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f)
            {
                moved = true;
            }

            Vector2 movementForce = Vector2.right * Input.GetAxis("Horizontal") * m_movementForce;
            
            if(m_powerPanel.MovementPower.PowerLevel == 3 && !grounded)
            {
                movementForce += Vector2.up * Input.GetAxis("Vertical") * m_movementForce * 0.25f;
            }
            m_rigidbody.AddForce(movementForce, ForceMode2D.Force);
        }

        if (m_powerPanel.MovementPower.PowerLevel > 1)
        {
            if (Input.GetButtonDown("Dash") && m_dashCooldown < m_timeSinceDashed)
            {
                Vector2 dashForce = Vector2.right * Input.GetAxis("Horizontal") * m_dashForce;
                m_rigidbody.AddForce(dashForce, ForceMode2D.Impulse);
                SpawnFadeout();
                m_dashing = true;
                m_timeSinceDashed = 0.0f;
                m_timeSinceSpawnedDashGhost = 0.0f;
                Audio.AUDIO.PlayClip(Audio.AUDIO.m_dash);
            }
        }

        if (m_powerPanel.JumpPower.PowerLevel > 0)
        {
            if (m_powerPanel.JumpPower.PowerLevel == 2 && grounded)
            {
                m_hasDoubleJumped = false;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (grounded)
                {
                    Vector2 jumpForce = Vector2.up * m_jumpForce;
                    m_rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
                    Audio.AUDIO.PlayClip(Audio.AUDIO.m_jump);
                }
                else if (m_powerPanel.JumpPower.PowerLevel == 2 && !m_hasDoubleJumped)
                {
                    Vector3 vel = m_rigidbody.velocity;
                    vel.y = 0;
                    m_rigidbody.velocity = vel;

                    Vector2 jumpForce = Vector2.up * m_doubleJumpForce;
                    m_rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
                    SpawnFadeout();
                    m_hasDoubleJumped = true;
                    Audio.AUDIO.PlayClip(Audio.AUDIO.m_jump);
                }

            }
            else if (Input.GetButton("Jump") && m_powerPanel.JumpPower.PowerLevel == 3)
            {
                m_rigidbody.gravityScale = 0.0f;

                //Drag
                m_rigidbody.AddForce(-m_rigidbody.velocity * 0.25f);
            }
            else
            {
                m_rigidbody.gravityScale = m_startingGravity;
            }
        }

        //Drag
        if(grounded && !moved)
        {
            
            float drag = Time.deltaTime * 60;
            float xSpeed = m_rigidbody.velocity.x - (m_rigidbody.velocity.x * drag);
            if (drag > 1)
            {
                xSpeed = 0;
            }


            Vector3 vel = m_rigidbody.velocity;
            vel.x = 0;
            m_rigidbody.velocity = vel;

            if (Mathf.Abs(vel.x) < 0.3f)
            {
                vel.x = 0;
                m_rigidbody.velocity = vel;
            }
        }

        if(m_rigidbody.velocity.magnitude > m_maxSpeed)
        {
            Vector3 vel = m_rigidbody.velocity;
            vel.Normalize();
            m_rigidbody.velocity = vel * m_maxSpeed;
        }

        if(!m_dashing)
        {
            if (m_rigidbody.velocity.x > m_maxHorizontalMoveSpeed)
            {
                Vector3 vel = m_rigidbody.velocity;
                vel.x = m_maxHorizontalMoveSpeed;
                m_rigidbody.velocity = vel;
            }
            if (m_rigidbody.velocity.x < -m_maxHorizontalMoveSpeed)
            {
                Vector3 vel = m_rigidbody.velocity;
                vel.x = -m_maxHorizontalMoveSpeed;
                m_rigidbody.velocity = vel;
            }
        }

    }

    public void Respawn()
    {
        if(!m_respawning)
        {
            m_respawning = true;
            m_timeRespawning = 0.0f;

            Analytics.CustomEvent("Dead", new Dictionary<string, object>{});
            Audio.AUDIO.PlayClip(Audio.AUDIO.m_death);
        }
    }
    public void ProcessRespawn()
    {
        m_timeRespawning += Time.deltaTime;
        m_rigidbody.velocity = new Vector3();
        m_rigidbody.isKinematic = true;

        if (m_timeRespawning < m_respawnTime * 0.5f)
        {
           var color = m_renderer.material.color;
            color.a = 1 - (m_timeRespawning / m_respawnTime * 0.5f);
            m_renderer.material.color = color;


        }
        else if(m_timeRespawning < m_respawnTime)
        {
            var color = m_renderer.material.color;
            color.a = ((m_timeRespawning - m_respawnTime * 0.5f) / m_respawnTime  );
            m_renderer.material.color = color;
            gameObject.transform.position = m_checkpoint.transform.position + new Vector3(0,1,0);
        }
        else
        {
            var color = m_renderer.material.color;
            color.a = 1;
            m_renderer.material.color = color;
            m_respawning = false;
            m_rigidbody.isKinematic = false;
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

    private GameObject SpawnFadeout()
    {
        var fadeout = Instantiate(m_fadeout);
        fadeout.transform.position = gameObject.transform.position;

        return fadeout;
    }
 }
