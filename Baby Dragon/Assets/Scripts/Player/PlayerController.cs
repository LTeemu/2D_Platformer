using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float jumpForce;
    public float movementSpeed;
    private bool jumpButtonDown = false;
    [SerializeField] float jumpBufferTime;
    private float jumpBufferTimer;
    [SerializeField] int startJumpCount;
    private int jumpCount;
    private bool jumpReduced = false;

    [Header("References")]
    [SerializeField] PolygonCollider2D playerCollider;
    Rigidbody2D rb;   
    private Joystick joystick;
    [SerializeField] ParticleSystem runParticle;
    //[SerializeField] ParticleSystem jumpParticle;
    private Animator animator;

    [Header("Collision")]
    public bool isGrounded;
    public bool isOnWall;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    public float velocityX;
    private Vector3 previous;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        playerCollider = transform.GetComponent<PolygonCollider2D>();
        joystick = FindObjectOfType<FixedJoystick>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {      
        GroundCheck();
        WallCheck();
        Jump();
        animator.SetFloat("Vertical", Mathf.Round(rb.velocity.y));
    }

    private void FixedUpdate()
    {
        moveHorizontal();
        velocityX = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;
    }

    void moveHorizontal()
    {
        float horizontal = joystick.Horizontal;
        if (horizontal != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                horizontal = Input.GetAxis("Horizontal");                
            }           

            if (horizontal > 0)
            {
                transform.localScale = new Vector3(0.6f, 0.6f, 1); ;
            }
            else
            {
                transform.localScale = new Vector3(-0.6f, 0.6f, 1);
            }

            if (!isOnWall)
            {
                transform.position += new Vector3(horizontal, 0, 0) * movementSpeed * Time.deltaTime;               
                if (isGrounded && Mathf.Abs(horizontal) > 0.7f)
                {
                    playRunParticle();
                }                
            }
            else
            {
                transform.position += Vector3.zero;
            }       
        }
        animator.SetFloat("Horizontal", Mathf.Abs(horizontal));
    }

    void Jump()
    {
        //PC jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPress();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpRelease();
        }

        //Jumpbuffer
        if (!isGrounded && jumpButtonDown)
        {
            jumpBufferTimer = jumpBufferTime;
        }
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        //Jump
        if (jumpButtonDown && jumpCount > 0 || jumpBufferTimer > 0 && jumpCount > 0 && isGrounded) {
            if (!jumpReduced) {
                animator.SetTrigger("Jump");
                rb.velocity = Vector2.up * jumpForce;
                //playJumpParticle();                          
                jumpCount -= 1;
                jumpReduced = true;              
            }            
        }

        //Resets
        if (isGrounded && !jumpButtonDown && jumpBufferTimer > 0) {
            jumpCount = startJumpCount - 1;           
        } else if (isGrounded && !jumpButtonDown && jumpBufferTimer <= 0) {
            jumpCount = startJumpCount;
        }
      
        if(!jumpButtonDown) {
            jumpReduced = false;
        }
    }

    void GroundCheck()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.size.y / 2), new Vector3(playerCollider.bounds.size.x, 0.1f), 0f, Vector2.down, 0.05f, groundLayer);
        if (raycastHit.collider != null)
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);      
        }
        else
        {           
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }
        

    void WallCheck()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center + Mathf.RoundToInt(joystick.Horizontal) * new Vector3(playerCollider.bounds.size.x / 2, 0), new Vector3(0.1f, playerCollider.bounds.size.y * 0.75f), 0f, transform.forward, 0.1f, wallLayer);
        if (raycastHit.collider != null)
        {
            isOnWall = true;
            animator.SetBool("isOnWall", true);
        }
        else
        {
            isOnWall = false;
            animator.SetBool("isOnWall", false);
        }
    }

    void playRunParticle()
    {
        runParticle.Play();
    }

    /*
    void playJumpParticle()
    {
        jumpParticle.Play();
    }
    */

    public void jumpRelease()
    {
        jumpButtonDown = false;
        jumpReduced = false;
    }

    public void jumpPress()
    {
        jumpButtonDown = true;        
    }

    private void OnDrawGizmos()
    {
        //GroundCheck       
        if(isGrounded && !isOnWall)
        {
            Gizmos.color = Color.green;
        } else if (!isGrounded && isOnWall) {
            Gizmos.color = Color.cyan;
        } else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireCube(playerCollider.bounds.center - new Vector3(0 , playerCollider.bounds.size.y / 2), new Vector3(playerCollider.bounds.size.x / 2, 0.1f));

       //WallCheck
       if(joystick)
        {
            Gizmos.DrawWireCube(playerCollider.bounds.center + Mathf.RoundToInt(joystick.Horizontal) * new Vector3(playerCollider.bounds.size.x / 2, 0),
            new Vector3(0.1f, playerCollider.bounds.size.y * 0.75f));
        }      
    }
}
