using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] GameObject InputManagerObj;
    ManageInputs inputManager;
    [SerializeField] float speed, jump;
    [SerializeField] float coyoteTime; // how late can I press jump after falling and still jump
    [SerializeField] float jumpInputStorage; // how early can the jump button be pressed before hitting ground
    [SerializeField] float jumpCancelChangeCoeff; // positive vertical speed loss coefficient (0 to 1) that applies when jump is released prematurely 

    float timeSinceLand, timeSinceJumpPress; 
    bool grounded;

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
        rb.velocity = new Vector2(speed * inputManager.GetJoystick().x, rb.velocity.y);
        
        //Debug.Log(rb.velocity);
        JumpHandler();
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && transform.position.y-transform.localScale.y * .49 > collision.transform.position.y + collision.transform.localScale.y*.45f)
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) grounded = false;
    }

    void JumpHandler()
    {
        if (inputManager.isJumpDown()) timeSinceJumpPress = 0;
        if (!grounded) timeSinceLand += Time.deltaTime;
        else timeSinceLand = 0;
    
        if (timeSinceJumpPress < jumpInputStorage && timeSinceLand < coyoteTime) Jump();
        timeSinceJumpPress += Time.deltaTime;


        if (inputManager.wasJumpReleased())
        {
            if (rb.velocity.y > 0 && timeSinceJumpPress < coyoteTime) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCancelChangeCoeff);
            
            timeSinceJumpPress = coyoteTime;
            
        }

    }

}
