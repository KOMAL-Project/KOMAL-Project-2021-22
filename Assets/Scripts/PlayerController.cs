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

    private Camera cam;
    private Rigidbody2D rb;

    private float timeSinceLand, timeSinceJumpPress, timeSinceJump; 
    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManagerObj.GetComponent<ManageInputs>();

        cam = Camera.main;

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity += new Vector2(speed * inputManager.GetJoystick().x, 0) * Time.deltaTime;

        rb.velocity = new Vector2(.75f * Mathf.Sign(rb.velocity.x) * Mathf.Min(maxSpeedX, Mathf.Abs(rb.velocity.x)), Mathf.Max(maxFalling, rb.velocity.y));

        //cam.gameObject.transform.position = transform.position + new Vector3(inputManager.GetJoystick().x, inputManager.GetJoystick().y, -10);
        cam.gameObject.GetComponent<Rigidbody>().velocity = 2*(transform.position - cam.gameObject.transform.position + 2 *new Vector3(inputManager.GetJoystick().x, inputManager.GetJoystick().y, -10));
        //Debug.Log(rb.velocity);
        JumpHandler();
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump);
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
        if (inputManager.pressedDown) timeSinceJumpPress = 0;

        if (!grounded) timeSinceLand += Time.deltaTime;
        else timeSinceLand = 0;

        if (timeSinceJumpPress < jumpInputStorage && timeSinceLand < coyoteTime && timeSinceJump > minTimeBetweenJumps)
        {
            timeSinceJump = 0;
            Jump();
        }

        timeSinceJumpPress += Time.deltaTime;
        timeSinceJump += Time.deltaTime;

        if (inputManager.justReleased)
        {
            Debug.Log("TEST");
            if (rb.velocity.y > 0 && timeSinceJumpPress < coyoteTime) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCancelChangeCoeff);
            
            timeSinceJumpPress = coyoteTime;
        }
    }

    public void EndJump()
    {
        //Debug.Log("TEST");
        if (rb.velocity.y > 0 && timeSinceJumpPress < coyoteTime) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCancelChangeCoeff);

        timeSinceJumpPress = coyoteTime;
    }

    void Bounce(float launchHeight, float maintainVelocityPercent) 
    {
        rb.velocity = new Vector2(rb.velocity.x, launchHeight + (-rb.velocity.y * maintainVelocityPercent));
    }
}
