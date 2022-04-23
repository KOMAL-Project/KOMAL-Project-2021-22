using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameObject InputManagerObj;
    private ManageInputs inputManager;  
    [SerializeField] float speed, jump, maxSpeedX, maxFalling;
    [SerializeField] float coyoteTime; // how late can I press jump after falling and still jump
    [SerializeField] private float jumpInputStorage; // how early can the jump button be pressed before hitting ground
    [SerializeField] private float jumpCancelChangeCoeff; // positive vertical speed loss coefficient (0 to 1) that applies when jump is released prematurely 
    [SerializeField] private float minTimeBetweenJumps = 0.25f;
    private GameObject checkpoint; // the transform of this object is where the player respawns.
    private Animator anim;

    private int score = 0;

    private Rigidbody2D rb;

    private float timeSinceLand, timeSinceJumpPress, timeSinceJump; 
    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManagerObj.GetComponent<ManageInputs>();

<<<<<<< HEAD
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
=======
        cam = Camera.main;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
        

>>>>>>> origin/level-one-art
    }

    // Update is called once per frame
    void Update()
    {

        

<<<<<<< HEAD
=======
        //cam.gameObject.transform.position = transform.position + new Vector3(inputManager.GetJoystick().x, inputManager.GetJoystick().y, -10);
        cam.gameObject.GetComponent<Rigidbody>().velocity = 2*(transform.position - cam.gameObject.transform.position + 2 *new Vector3(inputManager.GetJoystick().x, inputManager.GetJoystick().y, -10));

        // Handle Animations
        anim.SetFloat("X Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Y Speed", rb.velocity.y);
        if(Mathf.Abs(rb.velocity.x) > 0.05f) transform.localScale = rb.velocity.x > 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
>>>>>>> origin/level-one-art

        //Debug.Log(rb.velocity);
        
    }

    private void FixedUpdate()
    {
        rb.velocity += new Vector2(speed * inputManager.GetJoystick().x, 0) * Time.deltaTime;
        rb.velocity = new Vector2(.75f * Mathf.Sign(rb.velocity.x) * Mathf.Min(maxSpeedX, Mathf.Abs(rb.velocity.x)), Mathf.Max(maxFalling, rb.velocity.y));

        JumpHandler();

    }

    public void collectCoin()
    {
        score++;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Killing"))
        {
            if (checkpoint != null) transform.position = checkpoint.transform.position;
            else transform.position = new Vector3(0, 35, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) 
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                grounded = true;
                //Debug.Log(collision.gameObject.tag);
            }
            else if (collision.gameObject.CompareTag("BounceObject"))
            {
                umbrellaScript values = collision.GetComponent<umbrellaScript>();

                if (values) Bounce(values.launchSpeed, values.maintainVelocityPercent);
            }
            else if (collision.gameObject.CompareTag("Checkpoint"))
            {
                if(checkpoint != null) checkpoint.GetComponent<Animator>().SetBool("Lit", false);
                checkpoint = collision.gameObject;
                checkpoint.GetComponent<Animator>().SetBool("Lit", true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Player"))
        {
            grounded = true;
            //Debug.Log(collision.gameObject.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) grounded = false;
    }

    void JumpHandler()
    {
        if (inputManager.isJumpDown()) timeSinceJumpPress = 0;

        if (!grounded) timeSinceLand += Time.deltaTime;
        else timeSinceLand = 0;

        if (timeSinceJumpPress < jumpInputStorage && timeSinceLand < coyoteTime && timeSinceJump > minTimeBetweenJumps)
        {
            timeSinceJump = 0;
            Jump();
        }

        timeSinceJumpPress += Time.deltaTime;
        timeSinceJump += Time.deltaTime;

        if (inputManager.wasJumpReleased())
        {
            Debug.Log("TEST");
            if (rb.velocity.y > 0 && timeSinceJumpPress < coyoteTime) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCancelChangeCoeff);
            
            timeSinceJumpPress = coyoteTime;
        }
    }

    public void EndJump()
    {
        if (rb.velocity.y > 0 && timeSinceJumpPress < coyoteTime) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCancelChangeCoeff);

        timeSinceJumpPress = coyoteTime;
    }

    void Bounce(float launchHeight, float maintainVelocityPercent) 
    {
        rb.velocity = new Vector2(rb.velocity.x, launchHeight + (-rb.velocity.y * maintainVelocityPercent));
    }
}
