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

    private Rigidbody2D rb;

    private float timeSinceLand, timeSinceJumpPress, timeSinceJump; 
    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManagerObj.GetComponent<ManageInputs>();

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        rb.velocity += new Vector2(speed * inputManager.GetJoystick().x, 0) * Time.deltaTime;
        rb.velocity = new Vector2(.75f * Mathf.Sign(rb.velocity.x) * Mathf.Min(maxSpeedX, Mathf.Abs(rb.velocity.x)), Mathf.Max(maxFalling, rb.velocity.y));
        


        //Debug.Log(rb.velocity);
        JumpHandler();
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
            else if (collision.gameObject.CompareTag("Checkpoint")) checkpoint = collision.gameObject;
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
